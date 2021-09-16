using Ordering.API.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.API.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomer();
        Task<List<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(Int64 id);
        Task<List<Customer>> GetAllByEmailId(string email);
        Task<int> CreateAsync(Customer entity);
        Task<int> UpdateAsync(Customer entity);
        Task<int> DeleteAsync(Customer entity);

    }
}
