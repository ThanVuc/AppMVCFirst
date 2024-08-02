using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using AppMVC.Models.Abstract;
using AppMVC.Models.Product;

namespace AppMVC.Models.Blog
{
    public class Category : ACategory
    {
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
