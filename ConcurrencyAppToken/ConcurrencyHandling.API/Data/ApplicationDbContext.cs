using ConcurrencyHandling.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcurrencyHandling.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Booking>? Booking { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

    }
}
