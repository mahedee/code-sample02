using AuditLog.API.Models;
using AuditLog.API.Repositories.Interfaces;
using AuditLog.API.Services.Interfaces;

namespace AuditLog.API.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await _customerRepository.GetCustomers();
        }

        public async Task<Customer> GetCustomer(int id)
        {
            return await _customerRepository.GetCustomer(id);
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            var addedCustomer = await _customerRepository.AddCustomer(customer);
            return addedCustomer;
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            var updatedCustomer = await _customerRepository.UpdateCustomer(customer);
            return updatedCustomer;
        }

        public async Task<Customer> DeleteCustomer(int id)
        {
            var deletedCustomer = await _customerRepository.DeleteCustomer(id);
            return deletedCustomer;
        }
    }
}