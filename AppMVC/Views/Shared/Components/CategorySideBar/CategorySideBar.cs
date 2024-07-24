using AppMVC.Models.Blog;
using Microsoft.AspNetCore.Mvc;

namespace AppMVC.Views.Shared.Component.CategorySideBar
{
    public class CategorySideBarData
    {
        public List<Category> Categories { get; set; }
        public int level { get; set; }
        public string slug { get; set; }

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
