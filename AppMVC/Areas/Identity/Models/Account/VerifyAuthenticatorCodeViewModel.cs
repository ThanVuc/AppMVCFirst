// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace App.Areas.Identity.Models.AccountViewModels
{
    public class VerifyAuthenticatorCodeViewModel
    {
        [Required(ErrorMessage = "Require {0}")]
        [Display(Name = "Type the code has saved")]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this Browser?")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Remember login infomation?")]
        public bool RememberMe { get; set; }
    }
}
