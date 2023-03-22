using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTShop.Application.Common.Constants;
using MTShop.Core.Entities.Identity;

namespace MTShop.Infrastructure.Persistence.Configuration
{
    public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.ToTable(IdentityTableConstants.ApplicationUserRoles);
        }
    }
}
