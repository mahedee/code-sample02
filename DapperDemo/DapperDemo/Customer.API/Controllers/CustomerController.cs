using Microsoft.AspNetCore.Mvc;
using Ordering.API.Entities;
using Ordering.API.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<List<Customer>> Get()
        {
            return await _customerService.GetAllCustomer();
        }

        [HttpGet("{id}")]
        public async Task<Customer> GetAsync(Int64 id)
        {
            return await _customerService.GetByIdAsync(id);
        }

        [HttpGet("GetByEmail/{email}")]
        public async Task<List<Customer>> GetByEmailAsync(string email)
        {
            return await _customerService.GetAllByEmailId(email);
        }

        [HttpPost]
        public async Task<int> PostAsync([FromBody] Customer entity)
        {
            return await _customerService.CreateAsync(entity);
        }

        [HttpPut("EditCustomer/{id}")]
        public async Task<int> PutAsync(Int64 id, [FromBody] Customer entity)
        {
            try
            {
                if (entity.Id == id)
                {
                    return await _customerService.UpdateAsync(entity);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception exp)
            {
                throw new ApplicationException(exp.Message);
            }
        }

        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<int> DeleteAsync(Int64 id)
        {
            var entity = await _customerService.GetByIdAsync(id);
            return await _customerService.DeleteAsync(entity);
        }
    }
}
