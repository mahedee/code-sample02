using AuditLog.API.Repositories.Interfaces;
using AuditLog.API.Services.Interfaces;

namespace AuditLog.API.Services.Implementations
{
    public class ReportingService : IReportingService
    {
        private IReportingRepository _reportingRepository;
        public ReportingService(IReportingRepository reportingRepository)
        {
            _reportingRepository = reportingRepository;
        }
        public Task<IEnumerable<dynamic>> GetChangeLogDynamic(string EntityName)
        {
            return _reportingRepository.GetChangeLogDynamic(EntityName);
        }
    }
}
