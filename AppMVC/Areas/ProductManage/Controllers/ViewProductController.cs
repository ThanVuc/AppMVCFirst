using AppMVC.Areas.ProductManage.Services;
using AppMVC.Models;
using AppMVC.Models.Blog;
using AppMVC.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;

namespace AppMVC.Areas.Blog.Controllers
{
    [Area("ProductManage")]
    public class ViewProductController : Controller
    {
        private readonly AppDBContext _context;
        private readonly ILogger<ViewProductController> _logger;
        private readonly CartServices _cartServices;
        private readonly UserManager<AppUser> _userManager;

        public ViewProductController(AppDBContext context,
            ILogger<ViewProductController> logger,
            CartServices cartServices,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _logger = logger;
            _cartServices = cartServices;
            _userManager = userManager;
        }

        [Route("/product/{categoryslug?}")]
        public IActionResult Index([FromRoute(Name = "categoryslug")] string slug, [FromQuery(Name = "p")] int currentPage, [FromQuery] int size = 6)
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
                .Include(p => p.Seller)
                .Include(p => p.ProductCategoryProducts)
                .ThenInclude(pc => pc.CategoryProduct)
                .Include(p => p.ProductImages)
                .AsQueryable();

            products.OrderByDescending(p => p.DateUpdated);

            if (caterogy != null)
            {
                List<int> ids = new List<int>();
                ids.Add(caterogy.Id);
                caterogy.getCategoryChildIDs(ref ids);

                products = _context.ProductCategoryProducts
                    .Include(pc => pc.Product)
                    .ThenInclude(p => p.Seller)
                    .Where(pc => ids.Contains(pc.CategoryID))
                    .Select(pc => pc.Product);
            }


            //paging
            totalProduct = products.Count();
            pagingModel = new(currentPage, totalProduct, size, Url.Action("Index"));
            ViewBag.PagingModel = pagingModel;
            products.OrderByDescending(p => p.DateUpdated);

            //take paging item
            List<ProductModel> listProducts = pagingModel.TakePagingItem(products.ToList());


            return View(model: listProducts);
        }

        


        [Route("/product/{productslug}.html")]
        public async Task<IActionResult> Detail([FromRoute(Name = "productslug")] string slug)
        {
            if (slug == null)
            {
                return Content("Not Found a Posts");
            }

            ViewBag.categories = getCategories();

            var product = await _context.Products
                .Include(p => p.Seller)
                .Include(p => p.ProductCategoryProducts)
                .ThenInclude(p => p.CategoryProduct)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Slug == slug);

            ViewBag.category = product.ProductCategoryProducts.FirstOrDefault()?.CategoryProduct;


            if (product == null)
            {
                return Content($"Not found a Post have slug: {slug}");
            }

            return View(product);
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

        public async Task<IActionResult> AddToCart([FromQuery] int productId)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product == null)
            {
                return NotFound();
            }

            var cartList = _cartServices.GetCartItems();

            var cartItem = cartList
                .FirstOrDefault(c => c.Product.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity++;
            } else
            {
                var user = await _userManager.GetUserAsync(User);

                var cart = _context.Carts
                    .FirstOrDefault(c => c.Customer == user);

                cartList.Add(new ProductManage.Models.CartItem() { Product = product, Quantity = 1 });
            }

            _cartServices.SaveCart((List<ProductManage.Models.CartItem>)cartList);

            return RedirectToAction(nameof(ProductCart));
        }

        [HttpGet("/cart")]
        [Authorize]
        public IActionResult ProductCart()
        {
            return View((List<ProductManage.Models.CartItem>)_cartServices.GetCartItems());
        }

        [HttpPost("/cart/update")]
        public IActionResult UpdateCart([FromForm]int productid,[FromForm] int quantity)
        {
            var cart = _cartServices.GetCartItems();
            var cartItem = cart.FirstOrDefault(c => c.Product.ProductId == productid);
            
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
            }

            _cartServices.SaveCart(cart);

            return Ok();
        }

        [Route("/cart/delete")]
        public IActionResult RemoveCart([FromQuery]int productid)
        {
            var cartList = _cartServices.GetCartItems();
            var cartItem = cartList.FirstOrDefault(item => item.Product.ProductId == productid);

            if (cartItem != null)
            {
                cartList.Remove(cartItem);
            }

            _cartServices.SaveCart(cartList);

            return RedirectToAction(nameof(ProductCart));
        }
    }
}
