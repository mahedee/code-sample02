using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTShop.Application.Common.Constants;
using MTShop.Core.Entities.Admin.Identity;

namespace MTShop.Infrastructure.Persistence.Admin.Configuration
{
    public class AdminApplicationUserRoleConfiguration : IEntityTypeConfiguration<AdminApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<AdminApplicationUserRole> builder)
        {
            builder.ToTable(IdentityTableConstants.ApplicationUserRoles);
        }
    }
}
