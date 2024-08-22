using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppMVC.Models.Product
{
    public class Bill
    {
        [Key]
        public int BillItemId { get; set; }

        [Required]
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [Display(Name = "Product")]
        public ProductModel Product { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        [Display(Name = "Customer")]
        public AppUser Customer { get; set; }

        public int Quantity { get; set; }

        public DateTime BoughTime { get; set; }
    }
}
