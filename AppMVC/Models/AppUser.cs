using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AppMVC.Models
{
    public class AppUser : IdentityUser
    {
        [DataType("Nvarchar(200)")]
        [StringLength(maximumLength: 200, ErrorMessage = "Length <= 200")]
        public string? HomeAddress { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }

    }
}
