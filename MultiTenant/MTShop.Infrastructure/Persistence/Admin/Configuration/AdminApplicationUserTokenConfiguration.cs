using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTShop.Application.Common.Constants;
using MTShop.Core.Entities.Admin.Identity;

namespace MTShop.Infrastructure.Persistence.Admin.Configuration
{
    public class AdminApplicationUserTokenConfiguration : IEntityTypeConfiguration<AdminApplicationUserToken>
    {
        public void Configure(EntityTypeBuilder<AdminApplicationUserToken> builder)
        {
            builder.ToTable(IdentityTableConstants.ApplicationUserTokens);
        }
    }
}
