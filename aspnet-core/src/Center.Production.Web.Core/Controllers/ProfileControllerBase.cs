﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Center.Production.Authorization.Users.Profile.Dto;
using Center.Production.Dto;
using Center.Production.Storage;
using Center.Production.Web.Helpers;

namespace Center.Production.Web.Controllers
{
    public abstract class ProfileControllerBase : ProductionControllerBase
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;

        private const int MaxProfilePictureSize = 5242880; //5MB

        protected ProfileControllerBase(ITempFileCacheManager tempFileCacheManager)
        {
            _tempFileCacheManager = tempFileCacheManager;
        }

        public UploadProfilePictureOutput UploadProfilePicture(FileDto input)
        {
            try
            {
                var profilePictureFile = Request.Form.Files.First();

                //Check input
                if (profilePictureFile == null)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Change_Error"));
                }

                if (profilePictureFile.Length > MaxProfilePictureSize)
                {
                    throw new UserFriendlyException(L("ProfilePicture_Warn_SizeLimit", AppConsts.MaxProfilPictureBytesUserFriendlyValue));
                }

                byte[] fileBytes;
                using (var stream = profilePictureFile.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                if (!ImageFormatHelper.GetRawImageFormat(fileBytes).IsIn(ImageFormat.Jpeg, ImageFormat.Png, ImageFormat.Gif))
                {
                    throw new Exception(L("IncorrectImageFormat"));
                }

                _tempFileCacheManager.SetFile(input.FileToken, fileBytes);

                using (var bmpImage = new Bitmap(new MemoryStream(fileBytes)))
                {
                    return new UploadProfilePictureOutput
                    {
                        FileToken = input.FileToken,
                        FileName = input.FileName,
                        FileType = input.FileType,
                        Width = bmpImage.Width,
                        Height = bmpImage.Height
                    };
                }
            }
            catch (UserFriendlyException ex)
            {
                return new UploadProfilePictureOutput(new ErrorInfo(ex.Message));
            }
        }
    }
}