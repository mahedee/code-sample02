using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MTShop.Application;
using MTShop.Application.Common.Models;
using MTShop.Core.Entities.Identity;
using MTShop.Infrastructure.Persistence;
using System.Security.Claims;

namespace MTShop.Infrastructure.Seed
{
    public class MultitenantDbContextInitializer : IMultitenantDbContextInitializer
    {
        private readonly MultitenantDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<MultitenantDbContextInitializer> _logger;

        public MultitenantDbContextInitializer(MultitenantDbContext context, UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, ILogger<MultitenantDbContextInitializer> logger)
        {
            this._context = context ?? throw new NullReferenceException(nameof(context));
            this._userManager = userManager ?? throw new NullReferenceException(nameof(userManager));
            this._roleManager = roleManager ?? throw new NullReferenceException(nameof(roleManager));
            this._logger = logger ?? throw new NullReferenceException(nameof(logger));
        }

        public async Task SeedAsync(List<ApplicationRole>? defaultRoles, List<Users> defaultUsers, string tenantId)
        {
            try
            {
                await TrySeedAsync(defaultRoles, defaultUsers, tenantId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during seed process of database.");
                throw;
            }
        }

        private async Task TrySeedAsync(List<ApplicationRole>? defaultRoles, List<Users> defaultUsers, string tenantId)
        {
            // Create seed role
            if (defaultRoles != null)
            {
                foreach (var applicationRole in defaultRoles)
                {
                    var roleInfo = await _roleManager.FindByNameAsync(applicationRole.Name);

                    if (roleInfo is null)
                    {
                        applicationRole.TenantId = tenantId;
                        var result = await _roleManager.CreateAsync(applicationRole);
                        if (!result.Succeeded)
                            throw new Exception(result.Errors.First().Description);
                    }
                }
            }

            // create seed users
            if (defaultUsers != null)
            {
                foreach (var initialUserInfo in defaultUsers)
                {
                    var userInfo = await _userManager.FindByNameAsync(initialUserInfo.UserName);

                    if (userInfo is not null)
                        continue;

                    var user = new ApplicationUser
                    {
                        UserName = initialUserInfo.UserName,
                        Email = initialUserInfo.Email,
                        EmailConfirmed = true,
                        TenantId = tenantId,
                    };

                    var result = await _userManager.CreateAsync(user, initialUserInfo.Password);
                    if (!result.Succeeded)
                        throw new Exception(result.Errors.First().Description);

                    result = await _userManager.AddClaimsAsync(user, new Claim[]
                    {
                        new Claim(ClaimTypes.Name, initialUserInfo.UserClaims.UserName),
                        new Claim(ClaimTypes.Email,initialUserInfo.UserClaims.Email),
                        new Claim("TenantId", tenantId), // Tenant id adding as claim.
                    });

                    if (!result.Succeeded)
                        throw new Exception(result.Errors.First().Description);

                    await _userManager.AddToRoleAsync(user, initialUserInfo.Roles);
                }
            }
        }
    }
}
