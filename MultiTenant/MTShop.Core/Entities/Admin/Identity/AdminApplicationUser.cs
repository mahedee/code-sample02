using Microsoft.AspNetCore.Identity;

namespace MTShop.Core.Entities.Admin.Identity
{
    public class AdminApplicationUser : IdentityUser
    {
        public bool IsDeleted { get; set; } = false;
    }
}
