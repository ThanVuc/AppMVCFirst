using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMVC.Models.Product
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        [Display(Name = "Customer")]

        public AppUser Customer { get; set; }

        public List<CartItem> CartItems { get; set; }
    }
}
