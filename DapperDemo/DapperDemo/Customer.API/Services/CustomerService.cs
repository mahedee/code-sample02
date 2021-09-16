using Ordering.API.Entities;
using Ordering.API.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<int> CreateAsync(Customer entity)
        {
            return await _customerRepository.CreateAsync(entity);
        }

        public async Task<int> DeleteAsync(Customer entity)
        {
            return await _customerRepository.DeleteAsync(entity);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<List<Customer>> GetAllByEmailId(string email)
        {
            return await _customerRepository.GetAllByEmailId(email);
        }

        public async Task<List<Customer>> GetAllCustomer()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<Customer> GetByIdAsync(Int64 id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task<int> UpdateAsync(Customer entity)
        {
            return await _customerRepository.UpdateAsync(entity);
        }
    }
}
