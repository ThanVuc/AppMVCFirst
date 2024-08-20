using AppMVC.Models.Blog;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppMVC.Models.Product
{
    public class ProductModel
    {
        [Key]
        public int ProductId { set; get; }

        [Required(ErrorMessage = "Require Title")]
        [Display(Name = "Post Title")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} Size: {1} to {2}")]
        public string Title { set; get; }

        [Display(Name = "Short Description")]
        public string Description { set; get; }

        [Display(Name = "Url", Prompt = "Nhập hoặc để trống tự phát sinh theo Title")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} Size: {1} to {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Only char: [a-z0-9-]")]
        public string Slug { set; get; }

        [Display(Name = "Post Content")]
        public string Content { set; get; }

        [Display(Name = "Published")]
        public bool Published { set; get; }

        [DataType(DataType.Currency)]
        public double Price { set; get; }

        public int Number { get; set; }

        public List<ProductCategoryProduct> ProductCategoryProducts { get; set; }

        [Required]
        [Display(Name = "Seller")]
        public string SellerId { set; get; }
        [ForeignKey("SellerId")]
        [Display(Name = "Seller")]
        public AppUser Seller { set; get; }

        [Display(Name = "Created Date")]
        public DateTime DateCreated { set; get; }

        [Display(Name = "Updated Date")]
        public DateTime DateUpdated { set; get; }

        public List<ProductImage> ProductImages { get; set; }

        public List<CartItem> CartItems { get; set; }

        public List<Bill> BillList { get; set; }

    }
}
