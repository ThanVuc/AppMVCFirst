using AppMVC.Models.Blog;
using AppMVC.Models.Contact;
using AppMVC.Models.Product;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AppMVC.Models
{
    public class AppDBContext : IdentityDbContext<AppUser>
    {

        public DbSet<ContactModel> Contacts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ProductCategoryProduct> ProductCategoryProducts { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(c => c.Slug)
                .IsUnique();
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasIndex(p => p.Slug)
                .IsUnique();
            });

            modelBuilder.Entity<PostCategory>(entity =>
            {
                entity.HasKey(pc => new { pc.PostID, pc.CategoryID });
            });

            modelBuilder.Entity<CategoryProduct>(entity =>
            {
                entity.HasIndex(c => c.Slug)
                .IsUnique();
            });

            modelBuilder.Entity<ProductModel>(entity =>
            {
                entity.HasIndex(p => p.Slug)
                .IsUnique();
            });

            modelBuilder.Entity<ProductCategoryProduct>(entity =>
            {
                entity.HasKey(pc => new { pc.ProductID, pc.CategoryID });
            });

        }
    }
}
