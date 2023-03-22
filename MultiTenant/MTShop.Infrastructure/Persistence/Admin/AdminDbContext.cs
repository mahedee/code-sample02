using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MTShop.Core.Entities.Admin;
using MTShop.Core.Entities.Admin.Identity;
using MTShop.Infrastructure.Persistence.Admin.Configuration;

namespace MTShop.Infrastructure.Persistence.Admin
{

    public class AdminDbContext : IdentityDbContext<AdminApplicationUser, AdminApplicationRole, string, AdminApplicationUserClaim,
         AdminApplicationUserRole, AdminApplicationUserLogin, AdminApplicationUserRoleClaim, AdminApplicationUserToken>
    {

        private readonly IConfiguration _configuration;
        public AdminDbContext(DbContextOptions<AdminDbContext> options, IConfiguration configuration) : base (options)
        {
            this._configuration = configuration ?? throw new NullReferenceException(nameof (configuration));
        }

        public DbSet<TenantEntity> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("MTShop.Admin");


            // Apply configuration
            builder.ApplyConfiguration(new AdminApplicationRoleConfiguration());
            builder.ApplyConfiguration(new AdminApplicationUserClaimConfiguration());
            builder.ApplyConfiguration(new AdminApplicationUserConfiguration());
            builder.ApplyConfiguration(new AdminApplicationUserLoginConfiguration());
            builder.ApplyConfiguration(new AdminApplicationUserRoleClaimConfiguration());
            builder.ApplyConfiguration(new AdminApplicationUserRoleConfiguration());
            builder.ApplyConfiguration(new AdminApplicationUserTokenConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);

            var adminConnectionString = _configuration["AdminSettings:ConnectionString"];

            if(!String.IsNullOrEmpty(adminConnectionString))
            {
                optionsBuilder.UseSqlServer(adminConnectionString);
            }
        }
  


    }
}
