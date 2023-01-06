using Catalog.API.Model;

namespace Catalog.API.Db
{
    public class SeedDataProvider
    {
        public static void Initialize(CatalogContext catalogContext)
        {
            if(!catalogContext.CatalogItems.Any())
            {
                var catalogs = new List<CatalogItem>
                {
                    new CatalogItem
                    {
                        Name = "T-Shirt",
                        Description = "Cats Eye T-Shirt",
                        Price = 1000,
                        AvailableStock = 100,
                        RestockThreshold = 10
                    },

                    new CatalogItem
                    {
                        Name = "Samsung Mobile",
                        Description = "Samsung A30",
                        Price = 30000,
                        AvailableStock = 100,
                        RestockThreshold = 5
                    },

                    new CatalogItem
                    {
                        Name = "Meril Beauty Soap",
                        Description = "Beauty Soap",
                        Price = 40,
                        AvailableStock = 500,
                        RestockThreshold = 20
                    }
                };

                catalogContext.CatalogItems.AddRange(catalogs);
                catalogContext.SaveChanges();
            }
        }
    }
}
