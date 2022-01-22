using Microsoft.AspNetCore.Identity;
using Ordering.Application.Common.Interfaces;
using Ordering.Infrastructure.Identity;

namespace Ordering.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            //_signInManager = signInManager;
            //_roleManager = roleManager;
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
    }
}