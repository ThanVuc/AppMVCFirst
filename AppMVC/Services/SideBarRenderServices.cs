using AppMVC.Menu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Text;

namespace AppMVC.Services
{
    public class SideBarRenderServices
    {
        private List<SideBarItem> items = new List<SideBarItem>();

        public SideBarRenderServices()
        {

            //Database
            items.Add(new SideBarItem()
            {
                Area = "DatabaseManage",
                Controller = "Database",
                Action = "Index",
                Title = "Database",
                FontAweasome = "fa-solid fa-database",
                Type = TypeOfSideBar.NavBar,
                Items = null
            });

            //Contact
            items.Add(new SideBarItem()
            {
                Area = "ContactManage",
                Controller = "Contact",
                Action = "Index",
                Title = "Contact",
                FontAweasome = "fa-solid fa-address-book",
                Type = TypeOfSideBar.NavBar,
                Items = null
            });

            //File Manager
            items.Add(new SideBarItem()
            {
                Area = "FileManage",
                Controller = "FileSystem",
                Action = "Index",
                Title = "File Manager",
                FontAweasome = "fa-solid fa-file",
                Type = TypeOfSideBar.NavBar,
                Items = null
            });

            items.Add(new SideBarItem()
            {
                Type = TypeOfSideBar.Devided
            });

            //Identity
            items.Add(new SideBarItem()
            {
                Title = "Identity",
                FontAweasome = "fa-solid fa-fingerprint",
                Type = TypeOfSideBar.NavBar,
                CollapseId = "IdentityCollapse",
                Items = new List<SideBarItem>()
                {
                    new SideBarItem()
            {
                Area = "Identity",
                Controller = "Role",
                Action = "Index",
                Title = "Roles",
                FontAweasome = "fa-solid fa-person",
                Type = TypeOfSideBar.NavBar,
                Items = null
            },
                new SideBarItem()
            {
                Area = "Identity",
                Controller = "User",
                Action = "Index",
                Title = "Users",
                FontAweasome = "fa-regular fa-user",
                Type = TypeOfSideBar.NavBar,
                Items = null
            }
                }
            });

            //Blog
            items.Add(new SideBarItem()
            {
                Title = "Blog",
                FontAweasome = "fa-solid fa-blog",
                Type = TypeOfSideBar.NavBar,
                CollapseId = "BlogCollapse",
                Items = new List<SideBarItem>()
                {
                    new SideBarItem()
            {
                Area = "Blog",
                Controller = "Categories",
                Action = "Index",
                Title = "Category",
                FontAweasome = "fa-solid fa-list",
                Type = TypeOfSideBar.NavBar,
                Items = null
            },
                new SideBarItem()
            {
                Area = "Blog",
                Controller = "Post",
                Action = "Index",
                Title = "Post",
                FontAweasome = "fa-solid fa-envelopes-bulk",
                Type = TypeOfSideBar.NavBar,
                Items = null
            }
                }
            });


            //Product
            items.Add(new SideBarItem()
            {
                Title = "ProductManage",
                FontAweasome = "fa-solid fa-blog",
                Type = TypeOfSideBar.NavBar,
                CollapseId = "ProductCollapse",
                Items = new List<SideBarItem>()
                {
                    new SideBarItem()
            {
                Area = "ProductManage",
                Controller = "CategoryProduct",
                Action = "Index",
                Title = "Category Product",
                FontAweasome = "fa-solid fa-layer-group",
                Type = TypeOfSideBar.NavBar,
                Items = null
            },
                new SideBarItem()
            {
                Area = "ProductManage",
                Controller = "Product",
                Action = "Index",
                Title = "Product",
                FontAweasome = "fa-brands fa-product-hunt",
                Type = TypeOfSideBar.NavBar,
                Items = null
            }
                }
            });
        }

        public string RenderHtml(IUrlHelper urlHelper)
        {
            StringBuilder html = new StringBuilder();

            foreach (var item in items)
            {
                html.Append(item.RenderHtml(urlHelper));
            }

            return html.ToString();
        }
    }
}
