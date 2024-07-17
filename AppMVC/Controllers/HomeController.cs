using AppMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace AppMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("StatusErrors")]
        public IActionResult StatusErrors(int code)
        {
            var errName = (HttpStatusCode)code;
            return View(model: errName);
        }

        [Route("showfile")]
        public IActionResult ShowFile()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "pic.jpg");
            //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", fileName);
            _logger.LogInformation(filePath);
            if (System.IO.File.Exists(filePath))
            {
                var fileStream = new FileStream(filePath, FileMode.Open);
                return View(model: fileStream);
            }
            else
            {
                return View();
            }
        }

        [Route("downloadfile")]
        public IActionResult Downloadfile()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "abc.txt");
            //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", fileName);
            var fileName = Path.GetFileName(filePath);
            if (System.IO.File.Exists(filePath))
            {
                var file = System.IO.File.ReadAllBytes(filePath);
                return File(file, "application/octet-stream", fileName);
            }
            else
            {
                return NotFound();
            }
        }


    }
}
