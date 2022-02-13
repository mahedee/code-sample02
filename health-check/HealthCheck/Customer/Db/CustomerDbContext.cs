using Microsoft.EntityFrameworkCore;

namespace Customer.API.Db
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {

        }
    }
}
