using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MTShop.Application.Common.Constants;
using MTShop.Application.Common.Exceptions;
using MTShop.Application.Interfaces.Admin;
using MTShop.Core.Entities.Admin.Identity;

namespace MTShop.Infrastructure.Implementations
{
    public class AdminIdentityService : IAdminIdentityService
    {
        private readonly UserManager<AdminApplicationUser> _userManager;
        private readonly SignInManager<AdminApplicationUser> _signInManager;
        private readonly RoleManager<AdminApplicationRole> _roleManager;

        public AdminIdentityService(UserManager<AdminApplicationUser> userManager, SignInManager<AdminApplicationUser> signInManager,
            RoleManager<AdminApplicationRole> roleManager)
        {
            this._userManager = userManager ?? throw new NullReferenceException(nameof(userManager));
            this._signInManager = signInManager ?? throw new NullReferenceException(nameof(signInManager));
            this._roleManager = roleManager ?? throw new NullReferenceException(nameof(roleManager));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<bool> AssignUserToRoleAsync(string userName, IList<string> roles)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == userName);
            if (user == null)
                throw new NotFoundException("No user found with the given User Name");

            var result = await _userManager.AddToRolesAsync(user, roles);
            return result.Succeeded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<(IdentityResult, string roleId)> CreateRoleAsync(AdminApplicationRole role)
        {
            var result = await _roleManager.CreateAsync(role);
            return (result, role.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ValidationException"></exception>
        public async Task<(bool isSucceed, string userId)> CreateUserAsync(string userName, string password,
            string email, List<string> roles)
        {

            if (!(await IsUniqueUserName(userName)))
                throw new Exception("User already exists!");

            var user = new AdminApplicationUser
            {
                UserName = userName,
                Email = email,
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<(IdentityResult identityResult, string userId)> CreateUserAsync(AdminApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return (result, user.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="ValidationException"></exception>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="BadRequestException"></exception>
        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId)
                ?? throw new NotFoundException("User not found!");

            if (user.UserName == "admin" || user.UserName == "maidul")
                throw new BadRequestException("You cannot delete Adming and Maidul user");

            return (await _userManager.DeleteAsync(user)).Succeeded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<(string id, string userName, string email)>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.Select(x => new
            {
                x.Id,
                x.UserName,
                x.Email
            }).ToListAsync();

            return users.Select(user => (user.Id, user.UserName, user.Email)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<AdminApplicationRole> GetRoleAsync(string roleId)
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<AdminApplicationRole>> GetRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<AdminApplicationUser> GetUserAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<(string userId, string userName, IList<string> roles)> GetUserDetailsAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId)
                ?? throw new NotFoundException("User not found!");

            var roles = await _userManager.GetRolesAsync(user);
            return (user.Id, user.UserName, roles);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<string> GetUserIdAsync(string userName)
        {
            return (await _userManager?.Users?.FirstOrDefaultAsync(x => x.UserName == userName)).Id ??
                throw new NotFoundException("User not found");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<string> GetUserNameAsync(string userId)
        {
            return (await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId)).UserName ??
               throw new NotFoundException("User not found");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId) ??
                throw new NotFoundException("User not found!");

            return (await _userManager.GetRolesAsync(user)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId) ??
                throw new NotFoundException("User not found!");

            return await _userManager.IsInRoleAsync(user, role);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<bool> IsUniqueUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName) == null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<bool> SignInUserAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName) ??
                throw new NotFoundException("User not found!");

            return (await _signInManager.PasswordSignInAsync(user, password, true, false)).Succeeded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<(IdentityResult identityResult, string roleId)> UpdateRoleAsync(AdminApplicationRole role)
        {
            var result = await _roleManager.UpdateAsync(role);
            return (result, role.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<(IdentityResult identityResult, string userId)> UpdateUserAsync(AdminApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return (result, user.Id);
        }
    }
}
