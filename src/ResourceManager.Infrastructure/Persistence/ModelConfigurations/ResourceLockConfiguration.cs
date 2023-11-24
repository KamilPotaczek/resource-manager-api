using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceManager.Domain.Resources;

namespace ResourceManager.Infrastructure.Persistence.ModelConfigurations;

internal sealed class ResourceLockConfiguration : IEntityTypeConfiguration<ResourceLock>
{
    public void Configure(EntityTypeBuilder<ResourceLock> builder)
    {
        builder.Property<Guid>("ResourceId").IsRequired();
        
        builder.Property(rl => rl.Until)
            .HasColumnName("ActiveUntil");
    }
}