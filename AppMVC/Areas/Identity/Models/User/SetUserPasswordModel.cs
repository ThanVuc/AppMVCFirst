using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AppMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Areas.Identity.Models.UserViewModels
{
  public class SetUserPasswordModel
  {
      [Required(ErrorMessage = "Require {0}")]
      [StringLength(100, ErrorMessage = "{0} Length: {2} - {1} chars.", MinimumLength = 6)]
      [DataType(DataType.Password)]
      [Display(Name = "New Password")]
      public string NewPassword { get; set; }

      [DataType(DataType.Password)]
      [Display(Name = "Confirm Passowrd")]
      [Compare("NewPassword", ErrorMessage = "The confirm password have to duplicate with New Password")]
      public string ConfirmPassword { get; set; }


  }
}