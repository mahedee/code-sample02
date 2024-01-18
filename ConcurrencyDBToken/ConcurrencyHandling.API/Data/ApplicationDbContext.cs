using ConcurrencyHandling.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcurrencyHandling.API.Data
{
    public class ApplicationDbContext : DbContext
    {

        public virtual DbSet<Account> Accounts { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // IsConcurrencyToken() marks the property as a concurrency token.
            // This is used by EF Core to implement optimistic concurrency.
            // EF Core will use the value of the property in the WHERE clause of UPDATE and DELETE statements.

            modelBuilder.Entity<Account>()
                .Property(p => p.RowVersion).IsConcurrencyToken();
        }

    }
}
