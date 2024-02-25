using EFBulkBatch.Managers;
using EFBulkBatch.Models;
using Microsoft.AspNetCore.Mvc;

namespace EFBulkBatch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost("AddBulkData")]
        public async Task<IActionResult> AddBulkDataAsync()
        {
            var elapsedTime = await _employeeService.AddBulkDataAsync();
            return Ok(elapsedTime);
        }

        [HttpPut("UpdateBulkData")]
        public async Task<IActionResult> UpdateBulkDataAysnc()
        {
            var elapsedTime = await _employeeService.UpdateBulkDataAsync();
            return Ok(elapsedTime);

        }

        [HttpDelete("DeleteBulkData")]
        public async Task<IActionResult> DeleteBulkDataAsync()
        {
            var elapsedTime = await _employeeService.DeleteBulkDataAsync();
            return Ok(elapsedTime);
        }
    }
}
