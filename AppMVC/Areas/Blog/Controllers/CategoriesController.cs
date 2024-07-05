using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppMVC.Models;
using AppMVC.Models.Blog;
using Microsoft.AspNetCore.Authorization;

namespace AppMVC.Areas.Blog.Controllers
{
    [Area("Blog")]
    [Route("Categories/{action=index}")]
    [Authorize(policy: "HighLevelManage")]
    public class CategoriesController : Controller
    {
        private readonly AppDBContext _context;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(AppDBContext context, ILogger<CategoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Blog/Categories
        public async Task<IActionResult> Index()
        {
            //var appDBContext = _context.Categories.Include(c => c.ParentCategory);
            var qr = (from c in _context.Categories select c)
                .Include(c => c.ParentCategory)
                .Include(c => c.CategoryChildren);

            var categories = (await qr.ToListAsync())
                .Where(c => c.ParentCategory == null)
                .ToList();


            return View(categories);
        }

        // GET: Blog/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // Render priority by category
        void CreateSelectItems(List<Category> sources, List<Category> des, int level = 0)
        {
            string prefix = string.Concat(Enumerable.Repeat("--", level));
            foreach (var category in sources)
            {
                
                des.Add(new Category()
                {
                    Id = category.Id,
                    Title = prefix + " " + category.Title
                });
                if (category.CategoryChildren?.Count > 0)
                {
                    CreateSelectItems(category.CategoryChildren.ToList(),des,level+1);
                }
            }
        }

        async Task<SelectList> GenerateParentSelectList()
        {
            var qr = (from c in _context.Categories select c)
                .Include(c => c.ParentCategory)
                .Include(c => c.CategoryChildren);

            var categories = (await qr.ToListAsync())
                .Where(c => c.ParentCategory == null)
                .ToList();



            categories.Insert(0, new Category()
            {
                Id = -1,
                Title = "No Parent"
            });

            var items = new List<Category>();
            CreateSelectItems(categories, items);
            return new SelectList(items, "Id", "Title");
        }

        // GET: Blog/Categories/Create
        public async Task<IActionResult> Create()
        {
            var qr = (from c in _context.Categories select c)
                .Include(c => c.ParentCategory)
                .Include(c => c.CategoryChildren);

            var categories = (await qr.ToListAsync())
                .Where(c => c.ParentCategory == null)
                .ToList();



            categories.Insert(0, new Category()
            {
                Id = -1,
                Title = "No Parent"
            });

            var items = new List<Category>();
            CreateSelectItems(categories, items);

            ViewData["ParentCategoryId"] = await GenerateParentSelectList();
            return View();
        }

        // POST: Blog/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,Slug,ParentCategoryId")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ParentCategoryId == -1)
                {
                    category.ParentCategoryId = null;
                }
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ParentCategoryId"] = await GenerateParentSelectList();
            return View(category);
        }

        // GET: Blog/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }


            ViewData["ParentCategoryId"] = await GenerateParentSelectList();

            
            return View(category);
        }

        // POST: Blog/Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Slug,ParentCategoryId")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }
            
            if (category.ParentCategoryId == category.Id)
            {
                ModelState.AddModelError(string.Empty, "Please, Choose Another Parent Category");
            }


            if (ModelState.IsValid)
            {
                try
                {
                    if (category.ParentCategoryId == -1)
                    {
                        category.ParentCategoryId = null;
                    }

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["ParentCategoryId"] = await GenerateParentSelectList();
            return View(category);
        }

        // GET: Blog/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Blog/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories
                .Include(c => c.CategoryChildren)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (category == null)
            {
                return Content("Not Found");
            }

            foreach (var child in category.CategoryChildren)
            {
                child.ParentCategoryId = category.ParentCategoryId;
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
