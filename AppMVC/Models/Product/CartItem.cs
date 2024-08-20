using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMVC.Models.Product
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        [Required]
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [Display(Name = "Product")]
        public ProductModel Product { get; set; }

        [Required]
        [Display(Name = "Cart")]
        public int CartId { get; set; }

        [ForeignKey("CartId")]
        [Display(Name = "Cart")]
        public Cart Cart { get; set; }

        public int Quantity { get; set; }

    }
}
