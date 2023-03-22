using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTShop.Application.Common.Constants;
using MTShop.Core.Entities.Identity;

namespace MTShop.Infrastructure.Persistence.Configuration
{
    public class ApplicationUserRoleClaimConfiguration : IEntityTypeConfiguration<ApplicationUserRoleClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRoleClaim> builder)
        {
            //builder.HasKey(x => x.Id);
            builder.ToTable(IdentityTableConstants.ApplicationRoleClaims);
        }
    }
}
