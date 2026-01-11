using Microsoft.EntityFrameworkCore;
using Catalog.API.Models;

namespace Catalog.API.Data
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed some sample data
            modelBuilder.Entity<Product>().HasData(
                new Product 
                { 
                    Id = 1, 
                    Name = "Gaming Laptop", 
                    Description = "High-performance gaming laptop with RTX graphics", 
                    Price = 1299.99m, 
                    AvailableStock = 10, 
                    ReservedStock = 0,
                    MaxStockThreshold = 20 
                },
                new Product 
                { 
                    Id = 2, 
                    Name = "Wireless Mouse", 
                    Description = "Ergonomic wireless gaming mouse", 
                    Price = 79.99m, 
                    AvailableStock = 25, 
                    ReservedStock = 0,
                    MaxStockThreshold = 50 
                },
                new Product 
                { 
                    Id = 3, 
                    Name = "Mechanical Keyboard", 
                    Description = "RGB mechanical keyboard with Cherry MX switches", 
                    Price = 159.99m, 
                    AvailableStock = 15, 
                    ReservedStock = 0,
                    MaxStockThreshold = 30 
                }
            );

            // Add index for better performance
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name);
        }
    }
}
