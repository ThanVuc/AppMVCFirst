using System.ComponentModel.DataAnnotations;

namespace AppMVC.Areas.ProductManage.Models
{
    public class UploadFile
    {
        [Required(ErrorMessage = "Require at least one File")]
        [DataType(DataType.Upload)]
        [FileExtensions(Extensions = "png,jpg,jpeg,gif")]
        [Display(Name = "Select File Upload: ")]
        public IFormFile UploadImage { get; set; }
    }
}
