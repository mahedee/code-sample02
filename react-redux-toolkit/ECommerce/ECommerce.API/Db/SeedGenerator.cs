using ECommerce.API.Models;

namespace ECommerce.API.Db
{
    public class SeedGenerator
    {
        public static void SeedData(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ECommerceContext>();


                if (!context.Products.Any())
                {

                    context.Products.AddRange(
                        new Product
                        {
                            Name = "Smartphone",
                            Description = "A powerful and sleek smartphone with advanced features.",
                            Price = 699.99m,
                            StockQuantity = 50
                        },
                        new Product
                        {
                            Name = "Laptop",
                            Description = "A high-performance laptop for both work and entertainment.",
                            Price = 1299.99m,
                            StockQuantity = 25
                        },
                        new Product
                        {
                            Name = "Wireless Earbuds",
                            Description = "Premium wireless earbuds with noise-canceling technology.",
                            Price = 149.99m,
                            StockQuantity = 100
                        },
                        new Product
                        {
                            Name = "Smart Watch",
                            Description = "A stylish smartwatch with fitness tracking and app notifications.",
                            Price = 199.99m,
                            StockQuantity = 30
                        });
                }


                if (!context.Customers.Any())
                {
                    context.Customers.AddRange(
                        new Customer
                        {
                            Id = 1,
                            FirstName = "John",
                            LastName = "Doe",
                            Email = "john@example.com",
                            Phone = "555-1234",
                            BirthDate = new DateTime(1990, 5, 15)
                        },
                        new Customer
                        {
                            Id = 2,
                            FirstName = "Jane",
                            LastName = "Smith",
                            Email = "jane@example.com",
                            Phone = "555-5678",
                            BirthDate = new DateTime(1985, 8, 22)
                        },
                        new Customer
                        {
                            Id = 3,
                            FirstName = "Michael",
                            LastName = "Johnson",
                            Email = "michael@example.com",
                            Phone = "555-9876",
                            BirthDate = new DateTime(1992, 10, 10)
                        });
                }
                    context.SaveChanges();

            }
        }
    }
}
