using Microsoft.EntityFrameworkCore;
using Order.API.Models;

namespace Order.API.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }

        public DbSet<Models.Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Order>(entity =>
            {
                entity.HasIndex(e => e.ProductId);
                entity.Property(e => e.Status).HasMaxLength(20);
                entity.Property(e => e.ProductName).HasMaxLength(200);
            });
        }
    }
}
