using Catalog.API.Model;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Db
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Product>().HasData(
            //    new Product {Id = 1, Name = "Econo Pen", Description = "Econo", Price = 10, AvailableStock = 100, RestockThreshold = 20 },
            //    new Product {Id = 2, Name = "Casio Calculator", Description = "Casio Calculator", Price = 500, AvailableStock = 100, RestockThreshold = 10 });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Product> Products { get; set; }
    }
}
