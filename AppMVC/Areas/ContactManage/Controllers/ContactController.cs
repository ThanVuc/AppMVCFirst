using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppMVC.Models;
using AppMVC.Models.Contact;
using Microsoft.AspNetCore.Authorization;

namespace AppMVC.Areas.ContactManage.Controllers
{
    [Area("ContactManage")]
    public class ContactController : Controller
    {
        private readonly AppDBContext _context;

        public ContactController(AppDBContext context)
        {
            _context = context;
        }

        // GET: ContactManage/Contact
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contacts.ToListAsync());
        }

        // GET: ContactManage/Contact/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactModel = await _context.Contacts
                .FirstOrDefaultAsync(m => m.ContaxtID == id);
            if (contactModel == null)
            {
                return NotFound();
            }

            return View(contactModel);
        }

        // GET: ContactManage/Contact/Create
        [AllowAnonymous]
        [HttpGet]
        public IActionResult SendContact()
        {
            return View();
        }

        // POST: ContactManage/Contact/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> SendContact([Bind("ContaxtID,Name,Title,Content,Created")] ContactModel contactModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactModel);
        }

        // GET: ContactManage/Contact/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactModel = await _context.Contacts
                .FirstOrDefaultAsync(m => m.ContaxtID == id);
            if (contactModel == null)
            {
                return NotFound();
            }

            return View(contactModel);
        }

        // POST: ContactManage/Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactModel = await _context.Contacts.FindAsync(id);
            if (contactModel != null)
            {
                _context.Contacts.Remove(contactModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactModelExists(int id)
        {
            return _context.Contacts.Any(e => e.ContaxtID == id);
        }
    }
}
