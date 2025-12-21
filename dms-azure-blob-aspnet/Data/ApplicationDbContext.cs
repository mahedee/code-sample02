using DMS.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DMS.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Document entity
            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.OriginalFileName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.CustomerId).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UploadedBy).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ContentType).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UploadDate).IsRequired();
            });
        }
    }
}