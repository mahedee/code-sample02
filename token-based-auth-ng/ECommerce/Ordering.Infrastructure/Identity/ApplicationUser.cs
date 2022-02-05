
using Microsoft.AspNetCore.Identity;

namespace Ordering.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
