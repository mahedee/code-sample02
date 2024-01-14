using Z.EntityFramework.Plus;

namespace AuditLog.API.Models
{
    public class CustomAuditEntryProperty : AuditEntryProperty
    {
        // ApplicationName is a custom property that will be added to the AuditEntryProperty table
        // for storing the application name
        public string AppplicationName { get; set; }
    }
}
