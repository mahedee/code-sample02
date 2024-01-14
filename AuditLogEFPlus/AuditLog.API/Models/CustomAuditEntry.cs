using Z.EntityFramework.Plus;

namespace AuditLog.API.Models
{
    // CustomAuditEntry is a custom class that inherits from AuditEntry
    // It is used to add additional properties to the AuditEntry class
    public class CustomAuditEntry : AuditEntry
    {
        // ApplicationName is a custom property that will be added to the AuditEntry table for storing the application name
        public string AppplicationName { get; set; }
    }
}
