using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Interfaces;
using Ordering.Infrastructure.Identity;

namespace Ordering.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _roleManager = roleManager;
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if(!result.Succeeded)
            {
                //throw new ValidationException(result.Errors);
                throw new Exception(result.Errors.ToString());
            }
            return result.Succeeded;
        }


        // Return multiple value
        public async Task<(bool isSucceed, string userId)> CreateUserAsync(string userName, string password, string email, List<string> roles)
        {
            var user = new ApplicationUser()
            {
                UserName = userName,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                // Refactor to custom exception
                throw new Exception(result.Errors.ToString());
            }

            //var addUserRole = await _userManager.AddToRolesAsync(user, roles);
            //if (!addUserRole.Succeeded)
            //{
            //    throw new ValidationException(addUserRole.Errors);
            //}
            return (result.Succeeded, user.Id);
        }

        public async Task<(string userId, string UserName, IList<string> roles)> GetUserDetailsAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                //throw new NotFoundException("User not found");
                throw new Exception("User not found");
            }
            var roles = await _userManager.GetRolesAsync(user);
            return (user.Id, user.UserName, roles);
        }

        public async Task<string> GetUserIdAsync(string userName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null)
            {
                //throw new NotFoundException("User not found");
                throw new Exception("User not found");
            }
            return await _userManager.GetUserIdAsync(user);
        }

        public async Task<bool> SigninUserAsync(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, true, false);
            return result.Succeeded;
        }
    }
}