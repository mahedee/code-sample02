using Microsoft.EntityFrameworkCore;
using Product.API.Model;

namespace Product.API.Db
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        {

        }
        public DbSet<Product.API.Model.Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
