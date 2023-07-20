using Product.API.Model;

namespace Product.API.Db
{
    public class SeedGenerator
    {
        public static void SeedData(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ProductContext>();
                if (!context.Products.Any())
                {
                    context.Products.AddRange(
                        new Product.API.Model.Product
                        {
                            Name = "Cat's Shirt ",
                            ShortName = "Shirt",
                            Price = 1200,
                            ManufactureDate = DateTime.Today,
                            ExpiryDate = DateTime.Now.AddYears(5)
                        },
                         new Product.API.Model.Product
                         {
                             Name = "Lipton Tea Bag",
                             ShortName = "Tea",
                             Price = 150,
                             ManufactureDate = DateTime.Today,
                             ExpiryDate = DateTime.Now.AddYears(2)
                         }
                         );
                    context.SaveChanges();
                }

                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(
                        new Category
                        {
                            Name = "Beverages",
                            DisplayName = "Beverages"
                        },
                        new Category
                        {
                            Name = "Grossery",
                            DisplayName = "Grossery"
                        });
                    context.SaveChanges();
                }


            }
        }
    }
}
