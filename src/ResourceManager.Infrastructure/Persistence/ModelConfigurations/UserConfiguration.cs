using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResourceManager.Domain.Resources;
using ResourceManager.Domain.Users;

namespace ResourceManager.Infrastructure.Persistence.ModelConfigurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Id)
            .ValueGeneratedNever();

        builder.Property(user => user.Email)
            .HasColumnType("varchar(255)");

        builder.Property(e => e.Role)
            .HasConversion(
                userRole => userRole.Value,
                value => UserRole.FromValue(value));
        
        builder
            .HasMany<ResourceLock>()
            .WithOne();
        

        builder.HasData(new User("admin@rm.io", UserRole.Admin, Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")));
        builder.HasData(new User("slim.john@gmail.com", UserRole.User, Guid.Parse("fc851687-8e22-4fc4-a0be-8431a6e9d8ea")));
    }
}