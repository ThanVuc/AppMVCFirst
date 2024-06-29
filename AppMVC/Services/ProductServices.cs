using AppMVC.Areas.ProductManage.Models;

namespace AppMVC.Services
{
    public class ProductServices : List<Product>
    {

        public ProductServices()
        {
            this.AddRange(new Product[]
            {
                new Product(){ProductID = 1, ProductName = "SamSung", Price = 1000},
                new Product(){ProductID = 2, ProductName = "Nokia", Price = 1500},
                new Product(){ProductID = 3, ProductName = "XiaoMe", Price = 900},
                new Product(){ProductID = 4, ProductName = "Iphone", Price = 2000},
            });
        }
    }
}
