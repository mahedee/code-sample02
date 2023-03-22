using Microsoft.AspNetCore.Identity;

namespace MTShop.Core.Entities.Admin.Identity
{
    public class AdminApplicationRole : IdentityRole
    {
        public bool IsDeleted { get; set; } = false;
    }
}
