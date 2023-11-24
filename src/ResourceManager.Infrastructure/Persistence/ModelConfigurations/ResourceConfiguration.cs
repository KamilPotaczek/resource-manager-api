using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceManager.Domain.Resources;

namespace ResourceManager.Infrastructure.Persistence.ModelConfigurations;

internal sealed class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .ValueGeneratedNever();

        builder.Property(r => r.IsWithdrawn)
            .HasColumnName("Withdrawn")
            .HasColumnType("Bit")
            .HasDefaultValue(false);

        builder.HasOne(r => r.ResourceLock)
            .WithOne()
            .HasForeignKey<ResourceLock>(rl => rl.ResourceId);
    }
}