using System.ComponentModel.DataAnnotations;

namespace App.Areas.Identity.Models.RoleViewModels
{
  public class CreateRoleModel
    {
        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Require {0}")]
        [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} Length: {2} - {1} chars")]
        public string Name { get; set; }


    }
}
