using AppMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppMVC.Controllers
{
    [Route("SolarSystem/{action=index}")]
    public class PlanetController : Controller
    {
        private readonly PlanetServices _planets;
        private readonly ILogger<PlanetController> _logger;

        public PlanetController(PlanetServices planets, ILogger<PlanetController> logger)
        {
            _planets = planets;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(model: _planets);
        }

        [BindProperty(SupportsGet = true, Name = "action")]
        public string? Name { get; set; }

        public IActionResult Mercury()
        {
            var planet = _planets.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }

        public IActionResult Earth()
        {
            var planet = _planets.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }

        public IActionResult Jupiter()
        {
            var planet = _planets.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }

        public IActionResult Saturn()
        {
            var planet = _planets.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }

        public IActionResult Venus()
        {
            var planet = _planets.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }

        public IActionResult Uranus()
        {
            var planet = _planets.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }

        public IActionResult Nepturn()
        {
            var planet = _planets.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }

        public IActionResult Mars()
        {
            var planet = _planets.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }
    }
}
