using System.ComponentModel;

namespace AuditLog.API.AuditTrail.Enums
{
    // This enum is used to identify the names of applications that is being audited.
    public enum ApplicationEnum
    {
        [Description("AuditLog.API")]
        AuditLogAPP,

        [Description("ECommerce Application")]
        ECommerceAPP
    }
}
