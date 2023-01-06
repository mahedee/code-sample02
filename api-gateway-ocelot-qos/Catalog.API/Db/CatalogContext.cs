using Catalog.API.Model;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Db
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<CatalogItem> CatalogItems { get; set; }
    }
}
