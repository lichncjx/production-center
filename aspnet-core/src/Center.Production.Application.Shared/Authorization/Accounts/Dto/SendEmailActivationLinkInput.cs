﻿using System.ComponentModel.DataAnnotations;

namespace Center.Production.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}