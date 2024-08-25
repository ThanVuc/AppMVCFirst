using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace App.Areas.Identity.Models.RoleViewModels
{
  public class EditClaimModel
  {
    [Display(Name = "Type (Name) Claim")]
    [Required(ErrorMessage = "Phải nhập {0}")]
    [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} Length: {2} - {1} chars")]
    public string ClaimType { get; set; }

    [Display(Name = "Value")]
    [Required(ErrorMessage = "Require {0}")]
    [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} Length: {2} - {1} chars")]
    public string ClaimValue { get; set; }

    public IdentityRole role { get; set; }


  }
}
