using Microsoft.EntityFrameworkCore;
using MTShop.Application.Interfaces.Repositories;
using MTShop.Core.Entities;
using MTShop.Infrastructure.Persistence;

namespace MTShop.Infrastructure.Implementations.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MultitenantDbContext _multitenantDbContext;

        public CustomerRepository(MultitenantDbContext multitenantDbContext)
        {
            _multitenantDbContext = multitenantDbContext ?? throw new ArgumentNullException(nameof(multitenantDbContext));
        }
        public async Task<Customer> CreateAsync(Customer customer)
        {
            await _multitenantDbContext.Customers.AddAsync(customer);
            return customer;
        }

        public async Task<IReadOnlyList<Customer>> GetAllAsync()
        {
            return await _multitenantDbContext.Customers.ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(Guid id) => await _multitenantDbContext.Customers.FindAsync(id);
    }
}
