using System.ComponentModel.DataAnnotations;

namespace App.Areas.Identity.Models.UserViewModels
{
  public class AddUserClaimModel
  {
    [Display(Name = "Type (Name) Claim")]
    [Required(ErrorMessage = "Require {0}")]
    [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} Length: {2} - {1} chars")]
    public string ClaimType { get; set; }

    [Display(Name = "Value")]
    [Required(ErrorMessage = "Require {0}")]
    [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} Length: {2} to {1} chars")]
    public string ClaimValue { get; set; }

  }
}