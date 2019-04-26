﻿using Abp.AspNetCore.Mvc.Authorization;
using Center.Production.Authorization;
using Center.Production.Storage;
using Abp.BackgroundJobs;

namespace Center.Production.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}