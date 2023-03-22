using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTShop.Application.Common.Constants;
using MTShop.Core.Entities.Identity;

namespace MTShop.Infrastructure.Persistence.Configuration
{
    public class ApplicationUserLoginConfiguration : IEntityTypeConfiguration<ApplicationUserLogin>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserLogin> builder)
        {
            builder.ToTable(IdentityTableConstants.ApplicationUserLogins);
        }
    }
}
