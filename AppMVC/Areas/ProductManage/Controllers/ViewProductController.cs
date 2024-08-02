using AppMVC.Models;
using AppMVC.Models.Blog;
using AppMVC.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppMVC.Areas.Blog.Controllers
{
    [Area("ProductManage")]
    public class ViewProductController : Controller
    {
        private readonly AppDBContext _context;
        private readonly ILogger<ViewProductController> _logger;

        public ViewProductController(AppDBContext context, ILogger<ViewProductController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Route("/product/{categoryslug?}")]
        public IActionResult Index([FromRoute(Name = "categoryslug")] string slug, [FromQuery(Name = "p")] int currentPage, [FromQuery] int size = 5)
        {
            ViewBag.categories = getCategories();
            ViewBag.slug = slug;
            CategoryProduct caterogy = null;
            if (!string.IsNullOrEmpty(slug))
            {
                caterogy = _context.CategoryProducts
                    .Include(c => c.CategoryChildren)
                    .FirstOrDefault(c => c.Slug == slug);

                if (caterogy == null)
                {
                    return Content("Not Found Category");
                }

            }

            ViewBag.category = caterogy;

            //paging
            int totalProduct;
            PagingModel pagingModel;
            var products = _context.Products
                .Include(p => p.Author)
                .Include(p => p.ProductCategoryProducts)
                .ThenInclude(pc => pc.CategoryProduct)
                .AsQueryable();

            products.OrderByDescending(p => p.DateUpdated);

            if (caterogy != null)
            {
                List<int> ids = new List<int>();
                ids.Add(caterogy.Id);
                caterogy.getCategoryChildIDs(ref ids);

                products = _context.ProductCategoryProducts
                    .Include(pc => pc.Product)
                    .ThenInclude(p => p.Author)
                    .Where(pc => ids.Contains(pc.CategoryID))
                    .Select(pc => pc.Product);
            }


            //paging
            totalProduct = products.Count();
            pagingModel = new(currentPage, totalProduct, size, Url.Action("Index"));
            ViewBag.PagingModel = pagingModel;

            //take paging item
            List<ProductModel> listProducts = pagingModel.TakePagingItem(products.ToList());

            return View(model: listProducts);
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

        public List<CategoryProduct> getCategories()
        {
            var categories = _context.CategoryProducts
                .Include(c => c.CategoryChildren)
                .AsEnumerable()
                .Where(c => c.ParentCategory == null)
                .ToList();
            return categories;
        }
    }
}
