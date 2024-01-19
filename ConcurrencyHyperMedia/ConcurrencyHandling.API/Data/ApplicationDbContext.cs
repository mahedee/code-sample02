using ConcurrencyHandling.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ConcurrencyHandling.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Employee>? Employee { get; set; }
    }
}
