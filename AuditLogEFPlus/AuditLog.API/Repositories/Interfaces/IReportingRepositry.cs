namespace AuditLog.API.Repositories.Interfaces
{
    public interface IReportingRepositry
    {
        // Get all the changes for a particular entity response using Dynamic type
        Task<IEnumerable<dynamic>> GetChangeLogDynamic(string EntityName);
    }
}
