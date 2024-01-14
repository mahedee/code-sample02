using AuditLog.API.Persistence;
using AuditLog.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuditLog.API.Repositories.Implementations
{
    public class ReportingRepository : IReportingRepositry
    {
        private readonly AuditLogDBContext _context;

        public ReportingRepository(AuditLogDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<dynamic>> GetChangeLogDynamic(string EntityName)
        {
            String EntityTypeName = EntityName;

            var result = _context.AuditEntries
            .Where(a => a.EntityTypeName == EntityTypeName)
            .Join(_context.AuditEntryProperties,
            entry => entry.AuditEntryID,
            property => property.AuditEntryID,
                (entry, property) => new
                {
                    AuditEntryId = entry.AuditEntryID,
                    EntityTypeName = entry.EntityTypeName,
                    State = entry.State,
                    StateName = entry.StateName,
                    PropertyName = property.PropertyName,
                    OldValue = property.OldValue,
                    NewValue = property.NewValue,
                    CreatedBy = entry.CreatedBy,
                    CreatedDate = entry.CreatedDate
                }
                                                                                       )
            .OrderBy(result => result.CreatedDate)
            .Select(result => new
            {
                result.AuditEntryId,
                result.EntityTypeName,
                result.State,
                result.StateName,
                result.PropertyName,
                result.OldValue,
                result.NewValue,
                result.CreatedBy,
                result.CreatedDate
            });

            return await result.ToListAsync();
        }
    }
}
