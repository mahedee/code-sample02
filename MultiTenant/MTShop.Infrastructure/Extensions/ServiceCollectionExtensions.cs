using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MTShop.Application;
using MTShop.Application.Common.Models;
using MTShop.Application.Common.Settings;
using MTShop.Application.Interfaces;
using MTShop.Application.Interfaces.Admin;
using MTShop.Application.Interfaces.Repositories;
using MTShop.Core.Entities.Admin.Identity;
using MTShop.Core.Entities.Identity;
using MTShop.Infrastructure.Implementations;
using MTShop.Infrastructure.Implementations.Admin;
using MTShop.Infrastructure.Implementations.Repositories;
using MTShop.Infrastructure.Persistence;
using MTShop.Infrastructure.Persistence.Admin;
using MTShop.Infrastructure.Seed;

namespace MTShop.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static async Task<IServiceCollection> ConfigureAndMigrateAdminDB(this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddDbContext<AdminDbContext>(options =>
                options.UseSqlServer(x => x.MigrationsAssembly(typeof(AdminDbContext).Assembly.FullName)));


            services.AddIdentityCore<AdminApplicationUser>()
                       .AddRoles<AdminApplicationRole>()
                       .AddSignInManager()
                       .AddEntityFrameworkStores<AdminDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false; // For special character
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.User.RequireUniqueEmail = true;
            });

            using var scope = services.BuildServiceProvider().CreateScope();
            var adminDbContext = scope.ServiceProvider.GetRequiredService<AdminDbContext>();
            adminDbContext.Database.SetConnectionString(configuration["AdminSettings:ConnectionString"]);
            adminDbContext.Database.Migrate();

            

            var defaultUsers = services.GetOptions<AdminUserConfiguration>(nameof(AdminUserConfiguration)).Users;
            var defaultRoles = services.GetOptions<AdminUserConfiguration>(nameof(AdminUserConfiguration)).Roles;
            var defaultTenants = services.GetOptions<List<TenantsSeed>>(nameof(TenantsSeed));
            var initializer = scope.ServiceProvider.GetRequiredService<AdminDbContextInitializer>();
            await initializer.SeedAsync(defaultRoles, defaultUsers, defaultTenants);

            return services;
        }




        // Extension to handle configure and migrate tenant database as well as seeding information
        public static async Task<IServiceCollection> ConfigureAndMigrateTenantDatabase(this IServiceCollection services,
            IConfiguration configuration)
        {
            // Get default tenant information
            var options = services.GetOptions<TenantSettings>(nameof(TenantSettings));
            var defaultConnectionString = options.Defaults?.ConnectionString;
            var defaultDbProvider = options.Defaults?.DBProvider;

            if (defaultDbProvider?.ToLower() == "mssql-server")
            {
                services.AddDbContext<MultitenantDbContext>(options =>
                   options.UseSqlServer(x => x.MigrationsAssembly(typeof(MultitenantDbContext).Assembly.FullName)));

                services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<MultitenantDbContext>()
                .AddDefaultTokenProviders();

                services.Configure<IdentityOptions>(options =>
                {
                    // Default Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 15;
                    options.Lockout.AllowedForNewUsers = true;
                    //Default password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;
                    // Default SignIn settings.
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                    options.User.RequireUniqueEmail = true;
                });
            }

            var tenants = options.Tenants;
            foreach (var tenant in tenants)
            {
                string connectionString;
                if (String.IsNullOrEmpty(tenant.ConnectionString))
                {
                    connectionString = defaultConnectionString;
                }
                else
                {
                    connectionString = tenant.ConnectionString;
                }

                using var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<MultitenantDbContext>();
                dbContext.Database.SetConnectionString(connectionString);

                if (dbContext.Database.GetMigrations().Count() > 0)
                {
                    var defaultUsers = services.GetOptions<UserConfiguration>(nameof(UserConfiguration)).Users;
                    var defaultRoles = services.GetOptions<UserConfiguration>(nameof(UserConfiguration)).Roles;

                    var initializer = scope.ServiceProvider.GetRequiredService<MultitenantDbContextInitializer>();
                    dbContext.Database.Migrate();
                    await initializer.SeedAsync(defaultRoles, defaultUsers, tenant.TId);
                    //dbContext.Database.Migrate();
                }
            }

            return services;
        }

        public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(sectionName);
            var options = new T();
            section.Bind(options);
            return options;
        }


        // Register all services and interfaces
        public static IServiceCollection ConfigureService(this IServiceCollection services)
        {
            // AddScoped()- A new instance of a Scoped service is created once per request within the scope
            services.AddScoped<MultitenantDbContextInitializer>();
            services.AddScoped<AdminDbContextInitializer>();
            services.AddScoped<IMultitenantDbContextInitializer, MultitenantDbContextInitializer>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<IManageTenantRepository, ManageTenantRepository>();
            services.AddScoped<IAdminIdentityService, AdminIdentityService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAdminUnitOfWork, AdminUnitOfWork>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            //services.AddScoped<AdminDbContext>();

            return services;
        }

    }
}
