using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTShop.Application.Common.Constants;
using MTShop.Core.Entities.Admin.Identity;

namespace MTShop.Infrastructure.Persistence.Admin.Configuration
{
    public class AdminApplicationUserConfiguration : IEntityTypeConfiguration<AdminApplicationUser>
    {
        public void Configure(EntityTypeBuilder<AdminApplicationUser> builder)
        {
            //builder.HasKey(x => x.Id);
            builder.ToTable(IdentityTableConstants.ApplicationUsers);
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(256);
        }
    }
}
