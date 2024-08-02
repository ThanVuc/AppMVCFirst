using AppMVC.Models.Abstract;
using AppMVC.Models.Blog;
using AppMVC.Models.Product;
using Microsoft.AspNetCore.Mvc;

namespace AppMVC.Views.Shared.Component.CategorySideBar
{
    public class CategoryProductSideBarData : SideBarData
    {
        public List<CategoryProduct> Categories { get; set; }

    }

    [ViewComponent]
    public class CategoryProductSideBar : ViewComponent
    {
        
        public IViewComponentResult Invoke(CategoryProductSideBarData data)
        {
            return View("Product", model: data);
        }
    }
}
