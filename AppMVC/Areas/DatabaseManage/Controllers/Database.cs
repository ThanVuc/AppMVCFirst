using AppMVC.Areas.Identity.Data;
using AppMVC.Models;
using AppMVC.Models.Blog;
using AppMVC.Models.Product;
using Bogus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using System.Data.Entity;

namespace AppMVC.Areas.DatabaseManage.Controllers
{
    [Authorize(policy: "HighLevelManage")]
    [Area("DatabaseManage")]
    [Route("Database/{action=index}/{id?}")]
    public class Database : Controller
    {
        private readonly ILogger<Database> _logger;
        private readonly AppDBContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public Database(ILogger<Database> logger, AppDBContext context, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
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

        [TempData]
        public string StatusMessage { get; set; }
        
        [HttpGet]
        public async Task<IActionResult> SeedOriginData()
        {
            Console.WriteLine("Seed Data Is Run on");

            //seed role
            var listRole = typeof(RoleTemplate).GetProperties().ToList();
            foreach (var role in listRole)
            {
                if (!await _roleManager.RoleExistsAsync(role.Name))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role.Name));
                }
            }

            //seed data
            SeedPostCategory();
            SeedProduct();

            StatusMessage = "Seed Data Success";
            return RedirectToAction("index");
        }

        private void SeedPostCategory()
        {
            // Generate Category
            _context.Categories.RemoveRange(_context.Categories.Where(c => c.Content.Contains("[FakeData]")));

            var fackCategory = new Faker<Category>();
            int index = 1;
            fackCategory.RuleFor(c => c.Title, fk => $"Id {index++} " + fk.Lorem.Sentence(1, 2).Trim(','));
            fackCategory.RuleFor(c => c.Content, fk => fk.Lorem.Sentence(5).Trim(',') + " [FakeData]");
            fackCategory.RuleFor(c => c.Slug, fk => $"id{index++}" + fk.Lorem.Slug());

            var cate1 = fackCategory.Generate();
            var cate11 = fackCategory.Generate();
            var cate12 = fackCategory.Generate();
            var cate2 = fackCategory.Generate();
            var cate21 = fackCategory.Generate();
            var cate211 = fackCategory.Generate();

            cate11.ParentCategory = cate1;
            cate12.ParentCategory = cate1;
            cate21.ParentCategory = cate2;
            cate211.ParentCategory = cate21;

            var categories = new Category[] { cate1, cate11, cate12, cate2, cate21, cate211 };
            _context.AddRange(categories);
            _context.SaveChanges();

            // Generate Post + CatePost
            _context.Posts.RemoveRange(_context.Posts.Where(c => c.Content.Contains("[FakeData]")));

            var rCateIndex = new Random();
            int postNumber = 1;

            var user = _userManager.GetUserAsync(User).Result;
            var fakePost = new Faker<Post>();

            // Rule when generate
            fakePost.RuleFor(p => p.AuthorId, fakePost => user.Id);
            fakePost.RuleFor(p => p.Content, fakePost => fakePost.Lorem.Paragraph(7)+" [FakeData]");
            fakePost.RuleFor(p => p.DateCreated, fakePost => fakePost.Date.Between(new DateTime(2011,1,1), new DateTime(2024,7,1)));
            fakePost.RuleFor(p => p.Description, fakePost => fakePost.Lorem.Sentences(3));
            fakePost.RuleFor(p => p.Published, fakePost => true);
            fakePost.RuleFor(p => p.Slug, fakePost => fakePost.Lorem.Slug());
            fakePost.RuleFor(p => p.Title, fakePost => $"Post {postNumber++} " + fakePost.Lorem.Sentence(3,4).Trim('.'));

            List<Post> posts = new List<Post>();
            List<PostCategory> postCategories = new List<PostCategory>();

            for (int i=1; i<= 40; i++)
            {
                var post = fakePost.Generate();
                post.DateUpdated = post.DateCreated;
                posts.Add(post);
                postCategories.Add(new PostCategory()
                {
                    Post = post,
                    Category = categories[rCateIndex.Next(5)]
                });
            }

            _context.AddRange(posts);
            _context.AddRange(postCategories);

            _context.SaveChanges();

        }

        private void SeedProduct()
        {
            // Generate Category
            _context.CategoryProducts.RemoveRange(_context.CategoryProducts.Where(c => c.Content.Contains("[FakeData]")));

            var fackCategoryProduct = new Faker<CategoryProduct>();
            int index = 1;
            fackCategoryProduct.RuleFor(c => c.Title, fk => $"Category {index++} " + fk.Lorem.Sentence(1, 2).Trim(','));
            fackCategoryProduct.RuleFor(c => c.Content, fk => fk.Lorem.Sentence(5).Trim(',') + " [FakeData]");
            fackCategoryProduct.RuleFor(c => c.Slug, fk => $"category{index++}" + fk.Lorem.Slug());

            var cate1 = fackCategoryProduct.Generate();
            var cate11 = fackCategoryProduct.Generate();
            var cate12 = fackCategoryProduct.Generate();
            var cate2 = fackCategoryProduct.Generate();
            var cate21 = fackCategoryProduct.Generate();
            var cate211 = fackCategoryProduct.Generate();

            cate11.ParentCategory = cate1;
            cate12.ParentCategory = cate1;
            cate21.ParentCategory = cate2;
            cate211.ParentCategory = cate21;

            var categories = new CategoryProduct[] { cate1, cate11, cate12, cate2, cate21, cate211 };
            _context.AddRange(categories);
            _context.SaveChanges();

            // Generate Product + ProductCategoryProduct
            _context.Products.RemoveRange(_context.Products.Where(c => c.Content.Contains("[FakeData]")));

            var rCateIndex = new Random();
            int postNumber = 1;

            var user = _userManager.GetUserAsync(User).Result;
            var fakeProduct = new Faker<ProductModel>();

            // Rule when generate
            fakeProduct.RuleFor(p => p.SellerId, fakeProduct => user.Id);
            fakeProduct.RuleFor(p => p.Content, fakeProduct => fakeProduct.Lorem.Paragraph(7) + " [FakeData]");
            fakeProduct.RuleFor(p => p.DateCreated, fakeProduct => fakeProduct.Date.Between(new DateTime(2011, 1, 1), new DateTime(2024, 7, 1)));
            fakeProduct.RuleFor(p => p.Description, fakeProduct => fakeProduct.Lorem.Sentences(3));
            fakeProduct.RuleFor(p => p.Published, fakeProduct => true);
            fakeProduct.RuleFor(p => p.Slug, fakeProduct => fakeProduct.Lorem.Slug());
            fakeProduct.RuleFor(p => p.Title, fakeProduct => $"Product {postNumber++} " + fakeProduct.Lorem.Sentence(3, 4).Trim('.'));
            fakeProduct.RuleFor(p => p.Price, fakeProduct => 12.00);
            fakeProduct.RuleFor(p => p.Number, fakeProduct => fakeProduct.Random.Number(3,50));

            var products = new List<ProductModel>();
            // Product category Product
            var pcp = new List<ProductCategoryProduct>();

            for (int i = 1; i <= 40; i++)
            {
                var product = fakeProduct.Generate();
                product.DateUpdated = product.DateCreated;
                products.Add(product);
                pcp.Add(new ProductCategoryProduct()
                {

                    Product = product,
                    CategoryProduct = categories[rCateIndex.Next(5)]
                });
            }

            _context.AddRange(products);
            _context.AddRange(pcp);

            _context.SaveChanges();
        }

        // Fix Seed data -- Delete Bug 
        // Problem: Delete Top - Bottem lead to Contraints Exception
        // Delete Top can't handle Data constraints
        private void DeleteSelfReferences()
        {
        }

    }
}
