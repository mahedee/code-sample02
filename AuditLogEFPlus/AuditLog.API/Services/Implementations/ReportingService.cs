using AuditLog.API.Repositories.Interfaces;
using AuditLog.API.Services.Interfaces;

namespace AuditLog.API.Services.Implementations
{
    public class ReportingService : IReportingService
    {
        private readonly IReportingRepositry _reportingRepositry;

        public ReportingService(IReportingRepositry reportingRepositry)
        {
            _reportingRepositry = reportingRepositry;
        }

        public async Task<IEnumerable<dynamic>> GetChangeLogDynamic(string EntityName)
        {
            return await _reportingRepositry.GetChangeLogDynamic(EntityName);
        }
    }
}
