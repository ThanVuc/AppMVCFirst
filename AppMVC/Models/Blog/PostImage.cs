using AppMVC.Models.Product;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppMVC.Models.Blog
{
    public class PostImage
    {
        [Key]
        public int Id { get; set; }

        public string FileName { get; set; }

        public int PostId { get; set; }

        [ForeignKey("PostId")]
        public Post Post { get; set; }
    }
}
