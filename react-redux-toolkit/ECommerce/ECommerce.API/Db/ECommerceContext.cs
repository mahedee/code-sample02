using ECommerce.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Db
{
    public class ECommerceContext : DbContext
    {
        public ECommerceContext(DbContextOptions<ECommerceContext> options)
       : base(options)
        {

        }
        public DbSet<Product> Products { get;set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
