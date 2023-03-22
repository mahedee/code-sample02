using Microsoft.AspNetCore.Identity;
using MTShop.Core.Contracts;

namespace MTShop.Core.Entities.Identity
{
    public class ApplicationRole : IdentityRole, IMustHaveTenant
    {
        public string TenantId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
