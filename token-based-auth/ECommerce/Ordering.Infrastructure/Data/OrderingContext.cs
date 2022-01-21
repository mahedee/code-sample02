using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    // Context class for command
    public class OrderingContext : DbContext
    {
        public OrderingContext(DbContextOptions<OrderingContext> options) : base (options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}
