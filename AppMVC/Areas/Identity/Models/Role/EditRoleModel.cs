using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace App.Areas.Identity.Models.RoleViewModels
{
  public class EditRoleModel
    {
        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Require {0}")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} Length: {2} - {1} chars")]
        public string Name { get; set; }
        public List<IdentityRoleClaim<string>> Claims { get; set; }

        public IdentityRole role { get; set; }




    }
}
