using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTShop.Application.Common.Constants;
using MTShop.Core.Entities.Identity;

namespace MTShop.Infrastructure.Persistence.Configuration
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            //builder.HasKey(x => x.Id);
            builder.ToTable(IdentityTableConstants.ApplicationRoles);
        }
    }
}
