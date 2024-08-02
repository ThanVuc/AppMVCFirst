using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using AppMVC.Models.Abstract;

namespace AppMVC.Models.Product
{
    public class CategoryProduct : ACategory
    {
        // Các Category con
        public ICollection<CategoryProduct> CategoryChildren { get; set; }

        // Category cha (FKey)
        [Display(Name = "Parent Category")]
        public int? ParentCategoryId { get; set; }

        [ForeignKey("ParentCategoryId")]
        [Display(Name = "Parent Category")]


        public CategoryProduct ParentCategory { set; get; }

        public void getCategoryChildIDs(ref List<int> ids, CategoryProduct category = null)
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

        public List<CategoryProduct> GetParentCategories()
        {
            List<CategoryProduct> listParents = new List<CategoryProduct>();
            CategoryProduct category = this;
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
