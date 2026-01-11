using Microsoft.EntityFrameworkCore;
using Payment.API.Models;

namespace Payment.API.Data
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        {
        }

        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentTransaction>(entity =>
            {
                entity.HasIndex(e => e.OrderId);
                entity.HasIndex(e => e.TransactionId);
                entity.HasIndex(e => e.CustomerEmail);
                entity.Property(e => e.TransactionId).IsRequired();
            });
        }
    }
}
