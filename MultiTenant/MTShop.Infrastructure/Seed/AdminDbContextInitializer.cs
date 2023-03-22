using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MTShop.Application.Common.Models;
using MTShop.Core.Entities.Admin;
using MTShop.Core.Entities.Admin.Identity;
using MTShop.Infrastructure.Persistence.Admin;
using System.Security.Claims;

namespace MTShop.Infrastructure.Seed
{
    /// <summary>
    /// Seed data initialize and added at database.
    /// </summary>
    public class AdminDbContextInitializer
    {
        private readonly AdminDbContext _context;
        private readonly UserManager<AdminApplicationUser> _userManager;
        private readonly RoleManager<AdminApplicationRole> _roleManager;
        private readonly ILogger<AdminDbContextInitializer> _logger;

        public AdminDbContextInitializer(AdminDbContext context, UserManager<AdminApplicationUser> userManager,
            RoleManager<AdminApplicationRole> roleManager, ILogger<AdminDbContextInitializer> logger)
        {
            this._context = context ?? throw new NullReferenceException(nameof(context));
            this._userManager = userManager ?? throw new NullReferenceException(nameof(userManager));
            this._roleManager = roleManager ?? throw new NullReferenceException(nameof(roleManager));
            this._logger = logger ?? throw new NullReferenceException(nameof(logger));
        }

        public async Task SeedAsync(List<AdminApplicationRole>? defaultRoles, List<Users> defaultUsers,
            List<TenantsSeed>? defaultTenants)
        {
            Console.WriteLine("In a seed async method");

            try
            {
                await TrySeedAsync(defaultRoles, defaultUsers, defaultTenants);
            }
            catch(Exception exp)
            {
                _logger.LogError(exp, "An error occurred during seed process of database.");
                throw;
            }

        }

        private async Task TrySeedAsync(List<AdminApplicationRole>? defaultRoles, List<Users> defaultUsers, List<TenantsSeed> defaultTenants)
        {
            // create roles as seed data
            if (defaultRoles != null)
            {
                foreach (var applicationRole in defaultRoles)
                {
                    var roleInfo = await _roleManager.FindByNameAsync(applicationRole.Name);

                    if (roleInfo is null)
                    {

                        var result = await _roleManager.CreateAsync(applicationRole);
                        if (!result.Succeeded)
                            throw new Exception(result.Errors.First().Description);
                    }
                }
            }


            // Create seed user and assign user to role
            if (defaultUsers != null)
            {
                foreach (var initialUserInfo in defaultUsers)
                {
                    var userInfo = await _userManager.FindByNameAsync(initialUserInfo.UserName);

                    if (userInfo is not null)
                        continue;

                    var user = new AdminApplicationUser
                    {
                        UserName = initialUserInfo.UserName,
                        Email = initialUserInfo.Email,
                        EmailConfirmed = true,
                    };

                    var result = await _userManager.CreateAsync(user, initialUserInfo.Password);
                    if (!result.Succeeded)
                        throw new Exception(result.Errors.First().Description);

                    result = await _userManager.AddClaimsAsync(user, new Claim[]
                    {
                        new Claim(ClaimTypes.Name, initialUserInfo.UserClaims.UserName),
                        new Claim(ClaimTypes.Email,initialUserInfo.UserClaims.Email),
                        //new Claim(ClaimTypes.Role, tenantId),
                    });

                    if (!result.Succeeded)
                        throw new Exception(result.Errors.First().Description);

                    await _userManager.AddToRoleAsync(user, initialUserInfo.Roles);
                }
            }


            // Create default tenant
            if (defaultTenants != null)
            {

                if (_context.Tenants.Count() < 1)
                {
                    foreach (var tenant in defaultTenants)
                    {
                        var tenantEntity = new TenantEntity
                        {
                            ConnectionString = tenant.ConnectionString,
                            CreatedBy = tenant.CreatedBy,
                            DatabaseServer = tenant.DatabaseServer,
                            DBProvider = tenant.DBProvider,
                            ModifiedBy = tenant.ModifiedBy,
                            Password = tenant.Password,
                            TenantKey = tenant.TenantKey,
                            TenantName = tenant.TenantName,
                            UserName = tenant.UserName,
                        };
                        await _context.Tenants.AddAsync(tenantEntity);
                    }
                    await _context.SaveChangesAsync();
                }

            }


        }
    }
}
