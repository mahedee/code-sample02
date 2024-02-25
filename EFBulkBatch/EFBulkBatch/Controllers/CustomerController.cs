using EFBulkBatch.Managers;
using Microsoft.AspNetCore.Mvc;

namespace EFBulkBatch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly CustomerService _customerService;
        public CustomerController(CustomerService customerService) 
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("AddBulkData")]
        public async Task<IActionResult> AddBulkDataAsync()
        {
            var elapsedTime = await _customerService.AddBulkCustomerAsync();
            return Ok(elapsedTime);
        }

        [HttpPut("UpdateBulkData")] 
        public async Task<IActionResult> UpdateBulkDataAsync()
        {
            var elapsedTime = await _customerService.UpdateBulkCustomerAsync();
            return Ok(elapsedTime);
        }

        [HttpDelete("DeleteBulkData")]
        public async Task<IActionResult> DeleteBulkDataAsync()
        {
            var elapsedTime = await _customerService.DeleteBulkCustomerAsync();
            return Ok(elapsedTime);
        }
    }
}
