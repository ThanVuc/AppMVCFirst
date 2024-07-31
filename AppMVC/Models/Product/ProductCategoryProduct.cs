using System.ComponentModel.DataAnnotations.Schema;

namespace AppMVC.Models.Blog
{
    public class ProductCategoryProduct
    {
        public int ProductID { set; get; }

        public int CategoryID { set; get; }

        [ForeignKey("ProductID")]
        public Post Post { set; get; }

        [ForeignKey("CategoryID")]
        public Category Category { set; get; }
    }
}
