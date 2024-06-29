using AppMVC.Areas.ProductManage.Models;
using AppMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppMVC.Controllers
{
    public class FirstController : Controller
    {
        private readonly ILogger<FirstController> _logger;
        private readonly ProductServices _products;

        public FirstController(ILogger<FirstController> logger, ProductServices products)
        {
            _logger = logger;
            _products = products;
        }

        public string Index()
        {
            _logger.LogDebug("Debug");
            _logger.LogInformation("Information");
            _logger.LogWarning("Warning");
            _logger.LogError("Error");
            _logger.LogCritical("Critical");

            return "First Index";
        }

        public IActionResult ContentTest()
        {
            return Content("Content Test");
        }

        public IActionResult JsonTest()
        {
            return Json( new
            {
                Data = "JsonTest",
                Date = DateTime.Now
            });
        }

        public IActionResult FileTest()
        {
            //D:\Asp.Net\MVC\AppMVC\AppMVC\Image\Bird.jpg
            var bytes = System.IO.File.ReadAllBytes("Image/Bird.jpg");
            return File(bytes, "image/jpg");
        }

        public IActionResult LocalRedirectTest()
        {
            return LocalRedirect("/Home/Index");
        }

        public IActionResult RedirectTest()
        {
            return Redirect(@"https://github.com/new");
        }

        public IActionResult PassDataTest(int id)
        {
            ViewData["Title"] = "PassDataTest";
            ViewBag.Date = DateTime.Now.ToString("dd/MM/yyyy");

            Product product = _products.Where(p => p.ProductID == id).FirstOrDefault();

            if (product == null)
            {
                TempData["message"] = "Not Found Product!";
                return LocalRedirect("/Home/Index");
            }

            return View(product);
        }

    }
}
