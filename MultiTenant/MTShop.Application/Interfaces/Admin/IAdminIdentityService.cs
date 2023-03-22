using Microsoft.AspNetCore.Identity;
using MTShop.Core.Entities.Admin.Identity;

namespace MTShop.Application.Interfaces.Admin
{
    public interface IAdminIdentityService
    {
        Task<(IdentityResult identityResult, string userId)> CreateUserAsync(AdminApplicationUser user, string password);
        Task<(bool isSucceed, string userId)> CreateUserAsync(string userName, string password, string email, List<string> roles);
        Task<string> GetUserNameAsync(string userId);
        Task<string> GetUserIdAsync(string userName);
        Task<AdminApplicationUser> GetUserAsync(string userId);
        Task<(IdentityResult identityResult, string userId)> UpdateUserAsync(AdminApplicationUser user);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<List<string>> GetUserRolesAsync(string userId);
        Task<bool> SignInUserAsync(string userName, string password);
        Task<(string userId, string userName, IList<string> roles)> GetUserDetailsAsync(string userId);
        Task<bool> AssignUserToRoleAsync(string userName, IList<string> roles);
        Task<bool> IsUniqueUserName(string userName);
        Task<List<(string id, string userName, string email)>> GetAllUsersAsync();


        //Task<List<(string id, string roleName)>> GetRolesAsync();
        Task<List<AdminApplicationRole>> GetRolesAsync();
        Task<AdminApplicationRole> GetRoleAsync(string roleId);
        Task<(IdentityResult, string roleId)> CreateRoleAsync(AdminApplicationRole role);
        Task<(IdentityResult identityResult, string roleId)> UpdateRoleAsync(AdminApplicationRole role);
        Task<bool> DeleteRoleAsync(string roleId);

    }
}
