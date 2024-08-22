using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMVC.Models.Contact
{
    [Table("Contacts")]
    public class ContactModel
    {
        [Key]
        public int ContaxtID { get; set; }

        [StringLength(maximumLength: 100, MinimumLength = 2, ErrorMessage = "MinLength = 2 and MaxLength = 100")]
        [Required]
        public string Name { get; set; }

        [StringLength(maximumLength: 100, MinimumLength = 2, ErrorMessage = "MinLength = 2 and MaxLength = 100")]
        [Required]
        [DataType("NVarChar(100)")]
        public string Title { get; set; }
        [Required]
        [DataType("NText")]
        public string Content { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
    }
}
