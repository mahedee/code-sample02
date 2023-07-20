

using Customers.API.Model;

namespace Customers.API.Db
{
    public class SeedGenerator
    {
        public static void SeedData(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<CustomersContext>();


                if (!context.Customers.Any())
                {
                    context.Customers.AddRange(
                        new Customer
                        {
                            Name = "Scott Allen",
                            EmailAddress = "scott@gmail.com",
                            PhoneNo = "5068792493",
                            DOB = DateTime.Now.AddYears(-50),
                        },
                        new Customer
                        {
                            Name = "Graham Bell",
                            EmailAddress = "bell@outlook.com",
                            PhoneNo = "5068792494",
                            DOB = DateTime.Now.AddYears(-54),
                        },
                        new Customer
                        {
                            Name = "Adam Smith",
                            EmailAddress = "smith@yahoo.com",
                            PhoneNo = "5068792494",
                            DOB = DateTime.Now.AddYears(-54),
                        }); ;
                    context.SaveChanges();
                }

            }
        }
    }
}
