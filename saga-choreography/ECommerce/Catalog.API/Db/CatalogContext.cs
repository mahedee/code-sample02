using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Db
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {

        }

        public DbSet<CatalogItem> CatalogItems { get; set; }
    }
}
