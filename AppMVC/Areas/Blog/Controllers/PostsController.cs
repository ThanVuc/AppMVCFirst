using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppMVC.Models;
using AppMVC.Models.Blog;
using Microsoft.AspNetCore.Identity;
using App.Utilities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace AppMVC.Areas.Blog.Controllers
{
    [Area("Blog")]
    [Route("posts/{action=index}")]
    [Authorize(policy: "HighLevelManage")]
    public class PostsController : Controller
    {
        private readonly AppDBContext _context;
        private readonly ILogger<PostsController> _logger;
        private readonly UserManager<AppUser> _userManage;


        public PostsController(AppDBContext context, ILogger<PostsController> logger, UserManager<AppUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManage = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: Blog/Posts
        public async Task<IActionResult> Index([FromQuery(Name = "p")]int currentPage,[FromQuery]int size)
        {
            var allPosts = _context.Posts;
            int totalPost = await allPosts.CountAsync();

            PagingModel pagingModel = new (currentPage, totalPost, size, Url.Action("Index"));
            ViewBag.PagingModel = pagingModel;

            // Take Constraint Entity
            var post = await _context.Posts                
                .Include(p => p.Author)
                .Include(p => p.PostCategories)
                .ThenInclude(pc => pc.Category)
                .OrderByDescending(p => p.DateCreated)
                .ToListAsync();

            post = pagingModel.TakePagingItem(post);

            return View(post);
        }

        // GET: Blog/Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Blog/Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blog/Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm]int[] categoriesID,[Bind("Title,Description,Slug,Content,Published,AuthorId")] Post post)
        {
            if (ModelState.IsValid)
            {

                post.DateCreated = DateTime.Now.Date;
                post.DateUpdated = DateTime.Now.Date;

                if (post.Slug == null)
                {
                    post.Slug = AppUtilities.GenerateSlug(post.Title);
                }

                _context.Add(post);

                if (categoriesID != null)
                {
                    foreach (var cateID in categoriesID)
                    {
                        _context.Add(new PostCategory()
                        {
                            Post = post,
                            CategoryID = cateID
                        });
                    }
                }

                await _context.SaveChangesAsync();
                StatusMessage = "You've just create new posts";
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Blog/Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.Include(p => p.PostCategories).FirstOrDefaultAsync(p => p.PostId == id);
            
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int[] categoriesID, [Bind("PostId,Title,Description,Slug,Content,Published,AuthorId,PostCategories")] Post post)
        {
            if (id != post.PostId)
            {
                StatusMessage = "Not Found Post";
                return RedirectToAction("Index");
            }

            if (post.Slug == null)
            {
                post.Slug = AppUtilities.GenerateSlug(post.Title);
            }

            if (await _context.Posts.AnyAsync(p => p.Slug == post.Slug && p.PostId != post.PostId))
            {
                ModelState.AddModelError(string.Empty, "Duplicate Url, Please enter another Url");
                return View(post);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    
                    var postUpdate = await _context.Posts.Include(p => p.PostCategories)
                        .FirstOrDefaultAsync(p => p.PostId == id);

                    if (postUpdate == null)
                    {
                        StatusMessage = "Not Found Post";
                        return RedirectToAction("Index");
                    }

                    postUpdate.Title = post.Title;
                    postUpdate.Slug = post.Slug;
                    postUpdate.DateUpdated = DateTime.Now.Date;
                    postUpdate.Content = post.Content;
                    postUpdate.Description = post.Description;
                    postUpdate.Published = post.Published;

                    if (categoriesID == null)
                    {
                        categoriesID = new int[] { };
                    }

                    if (post.PostCategories == null)
                    {
                        return Content("Not Found");
                    }

                    // List Category Before Update
                    var oldCategories = postUpdate.PostCategories.Select(c => c.CategoryID).ToArray();

                    //After Update
                     var newCategories = categoriesID;

                    _logger.LogInformation("Old: " + string.Join(",", oldCategories));
                    _logger.LogInformation("New: " + string.Join(",", newCategories));


                    // Category need remove
                    var postCategoryRemove = from postCate in postUpdate.PostCategories
                                             where !newCategories.Contains(postCate.CategoryID)
                                             select postCate;
                    _logger.LogInformation("categoriesRemove: " + string.Join(",", postCategoryRemove));

                    _context.PostCategories.RemoveRange(postCategoryRemove);


                    // Category need add
                    var categoriesAdd = from CateID in newCategories
                                        where !oldCategories.Contains(CateID)
                                        select CateID;

                    _logger.LogInformation("categoriesAdd: " + string.Join(",", categoriesAdd));

                    foreach (var cateID in categoriesAdd)
                    {
                        _context.PostCategories.Add(new PostCategory()
                        {
                            PostID = id,
                            CategoryID = cateID
                        });
                    }

                    _context.Update(postUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
                    {
                        StatusMessage = "Not Found Post";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        throw;
                    }
                }
                StatusMessage = "You've just Updated post";
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Blog/Posts/Delete/5
        // CASCADE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            StatusMessage = "You've just deleted the post!";

            return View(post);
        }

        // POST: Blog/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
