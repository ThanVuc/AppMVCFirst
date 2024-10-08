﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace App.Areas.Identity.Models.AccountViewModels
{
    public class UseRecoveryCodeViewModel
    {
        
        [Required(ErrorMessage = "Require {0}")]
        [Display(Name = "Type the Recovery Code has saved")]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }
    }
}
