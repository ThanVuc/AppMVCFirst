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
using AppMVC.Models.Product;
using AppMVC.Areas.ProductManage.Models;

namespace AppMVC.Areas.ProductManage.Controllers
{
    [Area("ProductManage")]
    [Route("product/manager/{action=index}")]
    [Authorize(policy: "HighLevelManage")]
    public class ProductController : Controller
    {
        private readonly AppDBContext _context;
        private readonly ILogger<ProductController> _logger;
        private readonly UserManager<AppUser> _userManage;


        public ProductController(AppDBContext context, ILogger<ProductController> logger, UserManager<AppUser> userManager)
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
            var allProducts = _context.Products;
            int totalProduct = await allProducts.CountAsync();

            PagingModel pagingModel = new (currentPage, totalProduct, size, Url.Action("Index"));
            ViewBag.PagingModel = pagingModel;

            // Take Constraint Entity
            var product = await _context.Products
                .OrderByDescending(p => p.DateUpdated)
                .Include(p => p.Author)
                .Include(p => p.ProductCategoryProducts)
                .ThenInclude(pc => pc.CategoryProduct)
                .ToListAsync();

            product = pagingModel.TakePagingItem(product);

            return View(product);
        }

        // GET: Blog/Posts/Details/5
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Blog/Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blog/Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm]int[] categoriesID,[Bind("Title,Description,Slug,Content,Published,AuthorId")] ProductModel product)
        {
            if (ModelState.IsValid)
            {

                product.DateCreated = DateTime.Now.Date;
                product.DateUpdated = DateTime.Now.Date;

                if (product.Slug == null)
                {
                    product.Slug = AppUtilities.GenerateSlug(product.Title);
                }

                _context.Add(product);

                if (categoriesID != null)
                {
                    foreach (var cateID in categoriesID)
                    {
                        _context.Add(new ProductCategoryProduct()
                        {
                            Product = product,
                            CategoryID = cateID
                        });
                    }
                }

                await _context.SaveChangesAsync();
                StatusMessage = "You've just create new product";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Blog/Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(p => p.ProductCategoryProducts).FirstOrDefaultAsync(p => p.ProductId == id);
            
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int[] categoriesID, [Bind("ProductId,Title,Description,Slug,Content,Published,AuthorId,ProductCategoryProducts")] ProductModel product)
        {
            if (id != product.ProductId)
            {
                StatusMessage = "Not Found Product ID";
                return RedirectToAction("Index");
            }

            if (product.Slug == null)
            {
                product.Slug = AppUtilities.GenerateSlug(product.Title);
            }

            if (await _context.Posts.AnyAsync(p => p.Slug == product.Slug && p.PostId != product.ProductId))
            {
                ModelState.AddModelError(string.Empty, "Duplicate Url, Please enter another Url");
                return View(product);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var postUpdate = await _context.Products.Include(p => p.ProductCategoryProducts)
                        .FirstOrDefaultAsync(p => p.ProductId == id);

                    if (postUpdate == null)
                    {
                        StatusMessage = "Not Found Product";
                        return RedirectToAction("Index");
                    }

                    postUpdate.Title = product.Title;
                    postUpdate.Slug = product.Slug;
                    postUpdate.DateUpdated = DateTime.Now.Date;
                    postUpdate.Content = product.Content;
                    postUpdate.Description = product.Description;
                    postUpdate.Published = product.Published;

                    if (categoriesID == null)
                    {
                        categoriesID = new int[] { };
                    }

                    if (product.ProductCategoryProducts == null)
                    {
                        StatusMessage = "Not Found List Category";
                        return RedirectToAction("Index");
                    }

                    // List Category Before Update
                    var oldCategories = postUpdate.ProductCategoryProducts.Select(c => c.CategoryID).ToArray();

                    //After Update
                     var newCategories = categoriesID;


                    // Category need remove
                    var postCategoryRemove = from postCate in postUpdate.ProductCategoryProducts
                                             where !newCategories.Contains(postCate.CategoryID)
                                             select postCate;

                    _context.ProductCategoryProducts.RemoveRange(postCategoryRemove);


                    // Category need add
                    var categoriesAdd = from CateID in newCategories
                                        where !oldCategories.Contains(CateID)
                                        select CateID;


                    foreach (var cateID in categoriesAdd)
                    {
                        _context.ProductCategoryProducts.Add(new ProductCategoryProduct()
                        {
                            ProductID = id,
                            CategoryID = cateID
                        });
                    }

                    _context.Update(postUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(product.ProductId))
                    {
                        StatusMessage = "Product ID not Exist";
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
            return View(product);
        }

        // GET: Blog/Posts/Delete/5
        // CASCADE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Blog/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation(id.ToString());
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                StatusMessage = "Not found product!";
                return RedirectToAction(nameof(Index));
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            StatusMessage = "You've just deleted the product!";

            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }

        [HttpGet("/product/upload-image/{id?}")]
        public IActionResult UploadImage(int id)
        {
            var product = _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound("Product is not exist");
            }
            ViewData["product"] = product;
            
            return View(new UploadFile());
        }

        // Upload MVC
        [HttpPost("/product/upload-image/{id?}"), ActionName("UploadImage")]

        public async Task<IActionResult> UploadImage(int id, [Bind("UploadImage")]UploadFile f)
        {
            var product = _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound("Product is not exist");
            }

            ViewData["product"] = product;

            if (f != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                    + Path.GetExtension(f.UploadImage.FileName);

                var file = Path.Combine("image","product",fileName);

                using (FileStream fStream = new FileStream(file, FileMode.Create))
                {
                    await f.UploadImage.CopyToAsync(fStream);
                }

                // Upload Database
                _context.ProductImages.Add(new ProductImage()
                {
                    FileName = fileName,
                    Product = product
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("UploadImage",product.ProductId);
        }

        public IActionResult ListImage(int id)
        {
            var product = _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return Json(new
                {
                    success = 0,
                    message = "Product Not Found"
                });
            }

            var listData = product.ProductImages.Select(img => new
            {
                id = img.Id,
                path = "/image/product/" + img.FileName
            });

            return Json(new
            {
                success = 1,
                images = listData
            });
        }

        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id != null)
            {
                var item = await _context.ProductImages.FirstOrDefaultAsync(img => img.Id == id);
                // Delete In Folder
                string file = Path.Combine("image","product",item.FileName);
                System.IO.File.Delete(file);


                // Delete in DB
                _context.Remove(item);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        //Upload API
        [HttpPost("/product/api/upload-image/{id?}"), ActionName("UploadImageAPI")]

        public async Task<IActionResult> UploadImageAPI(int id, [Bind("UploadImage")] UploadFile f)
        {
            var product = _context.Products
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return Json(new
                {
                    status = 404,
                    message = "Not Found Product"
                });
            }

            if (f != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName())
                    + Path.GetExtension(f.UploadImage.FileName);

                var file = Path.Combine("image", "product", fileName);

                using (FileStream fStream = new FileStream(file, FileMode.Create))
                {
                    await f.UploadImage.CopyToAsync(fStream);
                }

                // Upload Database
                _context.ProductImages.Add(new ProductImage()
                {
                    FileName = fileName,
                    Product = product
                });
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

    }
}
