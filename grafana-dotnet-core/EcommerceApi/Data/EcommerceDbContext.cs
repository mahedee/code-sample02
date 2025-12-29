using EcommerceApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Data
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed some initial data for testing
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Laptop",
                    Description = "High-performance laptop for gaming and work",
                    Price = 999.99m,
                    StockQuantity = 50
                },
                new Product
                {
                    Id = 2,
                    Name = "Smartphone",
                    Description = "Latest smartphone with advanced features",
                    Price = 699.99m,
                    StockQuantity = 100
                },
                new Product
                {
                    Id = 3,
                    Name = "Headphones",
                    Description = "Wireless noise-canceling headphones",
                    Price = 199.99m,
                    StockQuantity = 75
                }
            );
        }
    }
}