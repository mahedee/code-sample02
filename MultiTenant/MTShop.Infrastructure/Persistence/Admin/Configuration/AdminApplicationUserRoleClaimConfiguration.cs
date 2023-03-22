using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTShop.Application.Common.Constants;
using MTShop.Core.Entities.Admin.Identity;

namespace MTShop.Infrastructure.Persistence.Admin.Configuration
{
    public class AdminApplicationUserRoleClaimConfiguration : IEntityTypeConfiguration<AdminApplicationUserRoleClaim>
    {
        public void Configure(EntityTypeBuilder<AdminApplicationUserRoleClaim> builder)
        {
            //builder.HasKey(x => x.Id);
            builder.ToTable(IdentityTableConstants.ApplicationRoleClaims);
        }
    }
}
