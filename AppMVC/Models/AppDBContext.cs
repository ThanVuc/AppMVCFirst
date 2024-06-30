using AppMVC.Models.Contact;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AppMVC.Models
{
    public class AppDBContext : DbContext
    {

        public DbSet<ContactModel> Contacts { get; set; }

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
        }
    }
}
