using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Mono.TextTemplating;

namespace AppMVC.Menu
{

    public enum TypeOfSideBar
    {
        Heading,
        Devided,
        NavBar
    }

    public class SideBarItem
    {
        public TypeOfSideBar Type { get; set; }

        public SideBarItem()
        {
        }

        public string Title { get; set; }

        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        public string FontAweasome;

        public string CollapseId { get; set; }

        public List<SideBarItem> Items { get; set; }

        public string GetUrlLink(IUrlHelper urlHelper)
        {
            return urlHelper.ActionLink(Action = Action, Controller = Controller, new { Area = Area });
        }
        public string RenderHtml(IUrlHelper urlHelper)
        {
            StringBuilder html = new StringBuilder();

            if (Type == TypeOfSideBar.Devided)
            {
                html.Append(" <div class=\"sb-sidenav-menu-devided\"><hr /></div>");
            } else if (Type == TypeOfSideBar.Heading)
            {
                html.Append($@"<div class=""sb-sidenav-menu-heading"">{Title}</div>");
            } else
            {
                if (Items == null)
                {
                    html.Append($@"<a class=""nav-link"" href=""{GetUrlLink(urlHelper)}"">
                        <div class=""sb-nav-link-icon""><i class=""{FontAweasome}""></i></div>
                        {Title}
                    </a>");
                } else
                {

                    var collapseItem = "";
                    foreach (var item in Items)
                    {
                        var urlLink = item.GetUrlLink(urlHelper);
                        collapseItem += $"<a class=\"nav-link\" href=\"{urlLink}\">{item.Title}</a>";
                    }


                    html.Append($@"
                    <div>
                            <a class=""nav-link collapsed"" href=""#"" data-bs-toggle=""collapse"" data-bs-target=""#{CollapseId}"" aria-expanded=""false"" aria-controls=""collapseLayouts"">
                                <div class=""sb-nav-link-icon""><i class=""{FontAweasome}""></i></div>
                                {Title}
                                <div class=""sb-sidenav-collapse-arrow""><i class=""fas fa-angle-down""></i></div>
                            </a>
                            <div class=""collapse"" id=""{CollapseId}"" aria-labelledby=""headingOne"" data-bs-parent=""#sidenavAccordion"">
                                <nav class=""sb-sidenav-menu-nested nav"">
                                    {collapseItem}
                                </nav>
                            </div>
                    </div>"
                    );
                }
                
            }

            return html.ToString();
        }

    }
}
