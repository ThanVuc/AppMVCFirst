using AppMVC.Models;
using AppMVC.Models.Blog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppMVC.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class ViewPostController : Controller
    {
        private readonly AppDBContext _context;
        private readonly ILogger<ViewPostController> _logger;

        public ViewPostController(AppDBContext context, ILogger<ViewPostController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Route("/post/{categoryslug?}")]
        public IActionResult Index([FromRoute(Name = "categoryslug")] string slug, [FromQuery(Name = "p")] int currentPage, [FromQuery] int size = 5)
        {
            ViewBag.categories = getCategories();
            ViewBag.slug = slug;
            Category caterogy = null;
            if (!string.IsNullOrEmpty(slug))
            {
                caterogy = _context.Categories
                    .Include(c => c.CategoryChildren)
                    .FirstOrDefault(c => c.Slug == slug);

                if (caterogy == null)
                {
                    return Content("Not Found Category");
                }

            }

            ViewBag.category = caterogy;

            //paging
            int totalPost;
            PagingModel pagingModel;
            var posts = _context.Posts
                .Include(p => p.Author)
                .Include(p => p.PostCategories)
                .ThenInclude(pc => pc.Category)
                .AsQueryable();

            posts.OrderByDescending(p => p.DateUpdated);

            if (caterogy != null)
            {
                List<int> ids = new List<int>();
                ids.Add(caterogy.Id);
                caterogy.getCategoryChildIDs(ref ids);
             
                posts = _context.PostCategories
                    .Include(pc => pc.Post)
                    .ThenInclude(p => p.Author)
                    .Where(pc => ids.Contains(pc.CategoryID))
                    .Select(pc => pc.Post);
            }


            //paging
            totalPost = posts.Count();
            pagingModel = new(currentPage, totalPost, size, Url.Action("Index"));
            ViewBag.PagingModel = pagingModel;

            //take paging item
            List<Post> listPosts = pagingModel.TakePagingItem(posts.ToList());

            return View(model: listPosts);
        }

        


        [Route("/post/{postslug}.html")]
        public async Task<IActionResult> Detail([FromRoute(Name = "postslug")] string slug)
        {
            if (slug == null)
            {
                return Content("Not Found a Posts");
            }

            ViewBag.categories = getCategories();

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.PostCategories)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(p => p.Slug == slug);

            ViewBag.category = post.PostCategories.FirstOrDefault()?.Category;


            if (post == null)
            {
                return Content($"Not found a Post have slug: {slug}");
            }

            return View(post);
        }

        public List<Category> getCategories()
        {
            var categories = _context.Categories
                .Include(c => c.CategoryChildren)
                .AsEnumerable()
                .Where(c => c.ParentCategory == null)
                .ToList();
            return categories;
        }
    }
}
