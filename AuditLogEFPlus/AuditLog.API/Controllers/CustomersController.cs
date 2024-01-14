using AuditLog.API.Models;
using AuditLog.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuditLog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await _customerService.GetCustomers();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<Customer> GetCustomer(int id)
        {
            return await _customerService.GetCustomer(id);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<Customer?> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
               return null;
            }
            try
            {
                return await _customerService.UpdateCustomer(customer);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            return await _customerService.AddCustomer(customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<Customer> DeleteCustomer(int id)
        {
            return await _customerService.DeleteCustomer(id);
        }

        private bool CustomerExists(int id)
        {
            return _customerService.GetCustomer(id) == null ? false : true;
        }
    }
}
