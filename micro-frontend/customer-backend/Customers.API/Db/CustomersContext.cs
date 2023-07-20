using Customers.API.Model;
using Microsoft.EntityFrameworkCore;

namespace Customers.API.Db
{
    public class CustomersContext : DbContext
    {
        public CustomersContext(DbContextOptions<CustomersContext> options)
            : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
    }
}
