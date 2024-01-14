using AuditLog.API.Models;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace AuditLog.API.Persistence
{
    public class AuditLogDBContext : DbContext
    {
        // AuditEntries and AuditEntryProperties are the tables that will be used to store the audit logs
        public DbSet<CustomAuditEntry> AuditEntries { get; set; }
        public DbSet<CustomAuditEntryProperty> AuditEntryProperties { get; set; }

        // Customers is the table that will be used to store the customer data
        public DbSet<Customer> Customers { get; set; }

        // AuditLogDBContext is the constructor that will be used to configure the audit log tables
        public AuditLogDBContext(DbContextOptions<AuditLogDBContext> options) : base(options)
        {
            // AutoSavePreAction is a delegate that will be used to save the audit logs to the database
            // It configures a pre-action to execute before the SaveChanges method is executed
            AuditManager.DefaultConfiguration.AutoSavePreAction = (context, audit) =>
            {
                ((AuditLogDBContext)context).AuditEntries.AddRange(audit.Entries.Cast<CustomAuditEntry>());
            };
        }


    }
}
