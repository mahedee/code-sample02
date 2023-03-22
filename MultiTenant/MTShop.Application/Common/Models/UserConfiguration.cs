
using MTShop.Core.Entities.Admin.Identity;
using MTShop.Core.Entities.Identity;

namespace MTShop.Application.Common.Models
{
    // For tenant db
    public record UserConfiguration
    {
        public List<ApplicationRole> Roles { get; set; }
        public List<Users> Users { get; set; }
    }

    // For admin DB
    public record AdminUserConfiguration
    {
        public List<AdminApplicationRole> Roles { get; set; }
        public List<Users> Users { get; set; }
    }

    public class Users
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Roles { get; set; }
        public UserClaims UserClaims { get; set; }
    }

    public record UserClaims
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
