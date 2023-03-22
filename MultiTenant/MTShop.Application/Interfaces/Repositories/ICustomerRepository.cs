using MTShop.Core.Entities;

namespace MTShop.Application.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<IReadOnlyList<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(Guid id);
        Task<Customer> CreateAsync(Customer customer);
    }
}
