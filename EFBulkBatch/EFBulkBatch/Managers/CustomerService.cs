using EFBulkBatch.Db;
using EFBulkBatch.Models;

namespace EFBulkBatch.Managers
{
    public class CustomerService
    {
        private readonly ApplicationDbContext _context;
        private DateTime _startTime;
        private TimeSpan _elapsedTime;
        public CustomerService(ApplicationDbContext context) 
        {
            _context = context;
        }


        public async Task<TimeSpan> AddBulkCustomerAsync()
        {
            // C# 9.0 feature: Target-typed new expressions
            List<Customer> customers = new();
            _startTime = DateTime.Now;

            for (int i = 0; i < 100000; i++)
            {
                customers.Add(new Customer
                {
                    Name = $"Customer {i}",
                    Email = $"Email {i}",
                    Phone = $"Phone {i}"
                });
            }

            // Use AddRange to add multiple entities
            _context.Customers.AddRange(customers);
            await _context.SaveChangesAsync();
            _elapsedTime = DateTime.Now - _startTime;
            return _elapsedTime;
        }

        public async Task<TimeSpan> UpdateBulkCustomerAsync()
        {
            // C# 9.0 feature: Target-typed new expressions
            List<Customer> customers = new();
            _startTime = DateTime.Now;
            for (int i = 0; i < 100000; i++)
            {
                customers.Add(new Customer
                {
                    Id = i + 1,
                    Name = $"Update Customer {i}",
                    Email = $"Update Email {i}",
                    Phone = $"Update Phone {i}"
                });
            }

            // Use UpdateRange to update multiple entities
            _context.Customers.UpdateRange(customers);
 
            await _context.SaveChangesAsync();

            _elapsedTime = DateTime.Now - _startTime;
            return _elapsedTime;
        }

        public async Task<TimeSpan> DeleteBulkCustomerAsync()
        {
            List<Customer> customers = _context.Customers.ToList();
            _startTime = DateTime.Now;

            // Use RemoveRange to delete multiple entities
            _context.Customers.RemoveRange(customers);
            await _context.SaveChangesAsync();
            _elapsedTime = DateTime.Now - _startTime;
            return _elapsedTime;
        }
    }
}
