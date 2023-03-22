using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MTShop.Application.Common.Constants;
using MTShop.Core.Entities.Admin.Identity;

namespace MTShop.Infrastructure.Persistence.Admin.Configuration
{
    public class AdminApplicationRoleConfiguration : IEntityTypeConfiguration<AdminApplicationRole>
    {
        public void Configure(EntityTypeBuilder<AdminApplicationRole> builder)
        {
            //throw new NotImplementedException();
            builder.ToTable(IdentityTableConstants.ApplicationRoles);
        }
    }
}
