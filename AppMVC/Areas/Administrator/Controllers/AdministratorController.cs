using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppMVC.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(policy: "HighLevelManage")]
    public class AdministratorController : Controller
    {
        [Route("/admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
