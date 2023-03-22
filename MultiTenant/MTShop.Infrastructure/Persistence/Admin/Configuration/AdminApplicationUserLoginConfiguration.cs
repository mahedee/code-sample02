using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTShop.Application.Common.Constants;
using MTShop.Core.Entities.Admin.Identity;

namespace MTShop.Infrastructure.Persistence.Admin.Configuration
{
    public class AdminApplicationUserLoginConfiguration : IEntityTypeConfiguration<AdminApplicationUserLogin>
    {
        public void Configure(EntityTypeBuilder<AdminApplicationUserLogin> builder)
        {
            builder.ToTable(IdentityTableConstants.ApplicationUserLogins);
        }
    }
}
