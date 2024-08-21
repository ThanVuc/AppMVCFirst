using AppMVC.Models.Product;

namespace AppMVC.Areas.ProductManage.Models
{
    public class CartItem
    {
        public ProductModel Product { get; set; }

        public int Quantity { get; set; }
    }
}
