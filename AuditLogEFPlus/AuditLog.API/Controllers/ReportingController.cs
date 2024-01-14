using AuditLog.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuditLog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportingController : ControllerBase
    {
        private readonly IReportingService _reportingService;

        public ReportingController(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        // GET: api/AuditTrailReport
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return NoContent();
        }

        // GET: api/AuditTrailReport/{EntityName}
        [HttpGet("GetChangeLog/{EntityName}")]
        public async Task<IActionResult> GetChangeLog(string EntityName)
        {
            return Ok(await _reportingService.GetChangeLogDynamic(EntityName));
        }
    }
}
