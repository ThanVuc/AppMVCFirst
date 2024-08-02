using AppMVC.Models.Abstract;
using AppMVC.Models.Blog;
using AppMVC.Models.Product;
using Microsoft.AspNetCore.Mvc;

namespace AppMVC.Views.Shared.Component.CategorySideBar
{
    public abstract class SideBarData
    {
        public int level { get; set; }
        public string slug { get; set; }
    }
     
    public class CategorySideBarData : SideBarData
    {
        public List<Category> Categories { get; set; }
    }

    [ViewComponent]
    public class CategorySideBar : ViewComponent
    {
        public IViewComponentResult Invoke(CategorySideBarData data)
        {
            return View(model: data);
        }
    }
}
