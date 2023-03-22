using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTShop.Application.Common.Constants;
using MTShop.Core.Entities.Admin.Identity;

namespace MTShop.Infrastructure.Persistence.Admin.Configuration
{
    public class AdminApplicationUserClaimConfiguration : IEntityTypeConfiguration<AdminApplicationUserClaim>
    {
        public void Configure(EntityTypeBuilder<AdminApplicationUserClaim> builder)
        {
            //builder.HasKey(x => x.Id);
            builder.ToTable(IdentityTableConstants.ApplicationUserClaims);
        }
    }
}
