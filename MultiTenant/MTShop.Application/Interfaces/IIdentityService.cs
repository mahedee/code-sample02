using Microsoft.AspNetCore.Identity;
using MTShop.Core.Entities.Identity;

namespace MTShop.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<(IdentityResult identityResult, string userId)> CreateUserAsync(ApplicationUser user, string password);
        Task<(bool isSucceed, string userId)> CreateUserAsync(string userName, string password, string email, string tenantId, List<string> roles);
        Task<string> GetUserNameAsync(string userId);
        Task<string> GetUserIdAsync(string userName);
        Task<ApplicationUser> GetUserAsync(string userId);
        Task<(IdentityResult identityResult, string userId)> UpdateUserAsync(ApplicationUser user);
        Task<bool> DeleteUserAsync(string userId);
        Task<bool> IsInRoleAsync(string userId, string role);
        Task<List<string>> GetUserRolesAsync(string userId);
        Task<bool> SignInUserAsync(string userName, string password);
        Task<(string userId, string userName, string tenantId, IList<string> roles)> GetUserDetailsAsync(string userId);
        Task<bool> AssignUserToRoleAsync(string userName, IList<string> roles);
        Task<bool> IsUniqueUserName(string userName);
        Task<List<(string id, string userName, string email, string tenantId)>> GetAllUsersAsync();


        //Task<List<(string id, string roleName)>> GetRolesAsync();
        Task<List<ApplicationRole>> GetRolesAsync();
        Task<ApplicationRole> GetRoleAsync(string roleId);
        Task<(IdentityResult, string roleId)> CreateRoleAsync(ApplicationRole role);
        Task<(IdentityResult identityResult, string roleId)> UpdateRoleAsync(ApplicationRole role);
        Task<bool> DeleteRoleAsync(string roleId);

    }
}
