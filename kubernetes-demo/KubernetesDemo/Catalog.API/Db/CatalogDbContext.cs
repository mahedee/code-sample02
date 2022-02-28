using Microsoft.EntityFrameworkCore;
using Catalog.API.Models;

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
