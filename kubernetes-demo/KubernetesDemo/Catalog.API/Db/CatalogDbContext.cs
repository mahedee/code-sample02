using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Db
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {

        }
        public DbSet<Catalog.API.Models.Product> Product { get; set; }
    }
}
