namespace AuditLog.API.Services.Interfaces
{
    public interface IReportingService
    {
        
        // Get all the changes for a particular entity response using Dynamic type
        Task<IEnumerable<dynamic>> GetChangeLogDynamic(string EntityName);
    }
}
