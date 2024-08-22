using AppMVC.Models.Blog;
using AppMVC.Models.Product;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppMVC.Models.Abstract
{
    public abstract class ACategory
    {
        [Key]
        public int Id { get; set; }

        // Tiều đề Category
        [Required(ErrorMessage = "Require")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} size: {1} to {2}")]
        [Display(Name = "Category Title")]
        public string Title { get; set; }

        // Nội dung, thông tin chi tiết về Category
        [DataType(DataType.Text)]
        [Display(Name = "Content")]
        public string Content { set; get; }

        //chuỗi Url
        [Required(ErrorMessage = "Require Slug")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} size: {1} to {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Only char: [a-z0-9-]")]
        [Display(Name = "Route Url")]
        public string Slug { set; get; }


        

    }
}
