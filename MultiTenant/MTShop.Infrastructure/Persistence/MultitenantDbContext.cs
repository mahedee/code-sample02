using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MTShop.Application.Interfaces;
using MTShop.Core.Entities;
using MTShop.Core.Entities.Identity;
using MTShop.Infrastructure.Persistence.Configuration;

namespace MTShop.Infrastructure.Persistence
{
    public class MultitenantDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, ApplicationUserClaim,
       ApplicationUserRole, ApplicationUserLogin, ApplicationUserRoleClaim, ApplicationUserToken>
    {
        private readonly ITenantService _tenantService;
        public string TenantId { get; set; }
        public string TenantName { get; set; }
        public MultitenantDbContext(DbContextOptions<MultitenantDbContext> options, ITenantService tenantService) 
            : base(options)
        {
            this._tenantService = tenantService;
            TenantId = _tenantService.GetTenant()?.TId;
            TenantName = _tenantService.GetTenant()?.Name;

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("MTShop.Tenants");
            builder.ApplyConfiguration(new ApplicationRoleConfiguration());
            builder.ApplyConfiguration(new ApplicationUserClaimConfiguration());
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new ApplicationUserLoginConfiguration());
            builder.ApplyConfiguration(new ApplicationUserRoleClaimConfiguration());
            builder.ApplyConfiguration(new ApplicationUserRoleConfiguration());
            builder.ApplyConfiguration(new ApplicationUserTokenConfiguration());

            builder.Entity<Product>().HasQueryFilter(x => x.TenantId == TenantId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            var tenantConnectionString = _tenantService.GetConnectionString();

            if(!String.IsNullOrEmpty(tenantConnectionString))
            {
                var dbProvider = _tenantService.GetDatabaseProvider();
                if(dbProvider.ToLower() == "mssql-server")
                {
                    optionsBuilder.UseSqlServer(_tenantService.GetConnectionString());
                }
            }
        }
    }
}
