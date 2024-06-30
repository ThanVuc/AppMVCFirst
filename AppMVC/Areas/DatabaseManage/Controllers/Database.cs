using AppMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppMVC.Areas.DatabaseManage.Controllers
{
    [Area("DatabaseManage")]
    [Route("Database/{action=index}/{id?}")]
    public class Database : Controller
    {
        private readonly ILogger<Database> _logger;
        private readonly AppDBContext _context;

        public Database(ILogger<Database> logger, AppDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult index()
        {
            if (_context.Database.CanConnect())
            {
                _context.Database.OpenConnection();
            }
            var info = new
            {
                DbName = _context.Database,
                Can_connect = _context.Database.CanConnect(),
                Connect = _context.Database.GetDbConnection(),
                Migrations = _context.Database.GetAppliedMigrations(),
                PendingMigration = _context.Database.GetPendingMigrations(),
                Schema = _context.Database.CanConnect() ? _context.Database.GetDbConnection().GetSchema("Tables") : null
            };

            return View(info);
        }

        [HttpGet]
        public IActionResult DeleteDB()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDBAsync()
        {
            var success = await _context.Database.EnsureDeletedAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> MigrateDB()
        {
            await _context.Database.MigrateAsync();

            return RedirectToAction("Index");
        }

    }
}
