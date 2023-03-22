using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MTShop.Application.Common.Constants;
using MTShop.Application.Common.Exceptions;
using MTShop.Application.Interfaces;
using MTShop.Core.Entities.Identity;

namespace MTShop.Infrastructure.Implementations
{
    /// <summary>
    /// Identity Repository to manage Tenant Authentication part of the databse.
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this._userManager = userManager ?? throw new NullReferenceException(nameof(userManager));
            this._signInManager = signInManager ?? throw new NullReferenceException(nameof(signInManager));
            this._roleManager = roleManager ?? throw new NullReferenceException(nameof(roleManager));
        }


        public async Task<bool> AssignUserToRoleAsync(string userName, IList<string> roles)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == userName);
            if (user == null)
                throw new NotFoundException("No user found with the given User Name");

            var result = await _userManager.AddToRolesAsync(user, roles);
            return result.Succeeded;
        }

        public async Task<(IdentityResult, string roleId)> CreateRoleAsync(ApplicationRole role)
        {
            var result = await _roleManager.CreateAsync(role);
            return (result, role.Id);
        }

        public async Task<(bool isSucceed, string userId)> CreateUserAsync(string userName, string password,
            string email, string tenantId, List<string> roles)
        {

            if (!(await IsUniqueUserName(userName)))
                throw new Exception("User already exists!");

            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                TenantId = tenantId,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new ValidationException(result.Errors);

            var addUserRole = await _userManager.AddToRolesAsync(user, roles);
            if (!addUserRole.Succeeded)
                throw new ValidationException(addUserRole.Errors);

            return (result.Succeeded, user.Id);
        }

        public async Task<(IdentityResult identityResult, string userId)> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return (result, user.Id);
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            var roleDetails = await _roleManager.FindByIdAsync(roleId)
                ?? throw new NotFoundException("Role not found");

            if (roleDetails.Name == UserRolesConstants.SuperAdmin)
                throw new BadRequestException("Super Admin Role Cannot be deleted!");

            var result = await _roleManager.DeleteAsync(roleDetails);

            if (!result.Succeeded)
                throw new ValidationException(result.Errors);

            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId)
                ?? throw new NotFoundException("User not found!");

            if (user.UserName == "admin" || user.UserName == "maidul")
                throw new BadRequestException("You cannot delete Adming and Maidul user");

            return (await _userManager.DeleteAsync(user)).Succeeded;
        }

        public async Task<List<(string id, string userName, string email, string tenantId)>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.Select(x => new
            {
                x.Id,
                x.UserName,
                x.Email,
                x.TenantId
            }).ToListAsync();

            return users.Select(user => (user.Id, user.UserName, user.Email, user.TenantId)).ToList();
        }

        public async Task<ApplicationRole> GetRoleAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        //public async Task<List<(string id, string roleName)>> GetRolesAsync()
        //{
        //    var roles = await _roleManager.Roles.Select(x => new
        //    {
        //        x.Id,
        //        x.Name
        //    }).ToListAsync();

        //    return roles.Select(role => (role.Id, role.Name)).ToList();
        //}

        public async Task<List<ApplicationRole>> GetRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<ApplicationUser> GetUserAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<(string userId, string userName, string tenantId, IList<string> roles)> GetUserDetailsAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId)
                ?? throw new NotFoundException("User not found!");

            var roles = await _userManager.GetRolesAsync(user);
            return (user.Id, user.UserName, user.TenantId, roles);
        }

        public async Task<string> GetUserIdAsync(string userName)
        {
            return (await _userManager?.Users?.FirstOrDefaultAsync(x => x.UserName == userName)).Id ??
                throw new NotFoundException("User not found");
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            return (await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId)).UserName ??
               throw new NotFoundException("User not found");
        }

        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId) ??
                throw new NotFoundException("User not found!");

            return (await _userManager.GetRolesAsync(user)).ToList();
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId) ??
                throw new NotFoundException("User not found!");

            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> IsUniqueUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName) == null;
        }

        public async Task<bool> SignInUserAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName) ??
                throw new NotFoundException("User not found!");

            return (await _signInManager.PasswordSignInAsync(user, password, true, false)).Succeeded;
        }

        public async Task<(IdentityResult identityResult, string roleId)> UpdateRoleAsync(ApplicationRole role)
        {
            var result = await _roleManager.UpdateAsync(role);
            return (result, role.Id);
        }

        public async Task<(IdentityResult identityResult, string userId)> UpdateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return (result, user.Id);
        }
    }
}
