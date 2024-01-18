using AuditLog.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditLog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportingController : ControllerBase
    {
        private IReportingService _reportingService;
        public ReportingController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }
        [HttpGet("GetChangeLog/{EntityName}")]
        public async Task<IActionResult> GetChangeLog(string EntityName)
        {
            var result = await _reportingService.GetChangeLogDynamic(EntityName);
            return Ok(result);
        }
    }
}
