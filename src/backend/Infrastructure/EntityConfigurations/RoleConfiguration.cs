using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasData(
            new Role { Id = Guid.NewGuid(), Name = "Registered", Permissions = [Permissions.User] },
            new Role { Id = Guid.NewGuid(), Name = "Administrator", Permissions = [.. Enum.GetValues<Permissions>()] });
    }
}
