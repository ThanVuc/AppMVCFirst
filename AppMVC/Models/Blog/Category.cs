using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace AppMVC.Models.Blog
{
    public class Category
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

        // Các Category con
        public ICollection<Category> CategoryChildren { get; set; }

        // Category cha (FKey)
        [Display(Name = "Parent Category")]
        public int? ParentCategoryId { get; set; }

        [ForeignKey("ParentCategoryId")]
        [Display(Name = "Parent Category")]


        public Category ParentCategory { set; get; }

        public void getCategoryChildIDs(ref List<int> ids, Category category = null)
        {
            if (category == null)
            {
                category = this;
            }

            foreach (var categoryChild in category.CategoryChildren)
            {
                ids.Add(categoryChild.Id);
                if (categoryChild.CategoryChildren.Count > 0)
                {
                    getCategoryChildIDs(ref ids, categoryChild);
                }
            }
        }

        public List<Category> GetParentCategories()
        {
            List<Category> listParents = new List<Category>();
            Category category = this;
            while (category.ParentCategory != null)
            {
                listParents.Add(category);
                category = category.ParentCategory;
            }

            listParents.Add(category);
            listParents.Reverse();

            return listParents;

        }

    }
}
