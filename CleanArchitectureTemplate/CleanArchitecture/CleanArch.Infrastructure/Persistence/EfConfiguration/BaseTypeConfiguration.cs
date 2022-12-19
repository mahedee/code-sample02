using CleanArch.Core.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArch.Infrastructure.Persistence.EfConfiguration
{
    public class BaseTypeConfiguration<TItem> : IEntityTypeConfiguration<TItem> where TItem : BaseEntity<long>
    {
        public virtual void Configure(EntityTypeBuilder<TItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AuthorizeStatus).HasMaxLength(5);
            builder.Property(x => x.AuthorizedBy).HasMaxLength(100);
            builder.Property(x => x.CreatedBy).HasMaxLength(100);
            builder.Property(x => x.LastModifiedBy).HasMaxLength(100);
        }
    }
}
