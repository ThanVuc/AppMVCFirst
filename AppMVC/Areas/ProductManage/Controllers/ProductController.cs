using AppMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppMVC.Areas.ProductManage.Controllers
{
    [Area("ProductManage")]
    public class ProductController : Controller
    {
        private readonly ProductServices _products;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ProductServices products, ILogger<ProductController> logger)
        {
            _products = products;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var products = _products.OrderBy(p => p.ProductName).ToList();
            return View(model: products);
        }

        [HttpGet("getpage")]
        public IActionResult TestHttpGet()
        {
            return Content("Http Get Succesful!");
        }

    }
}
