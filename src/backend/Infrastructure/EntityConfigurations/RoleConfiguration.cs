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

        builder.HasMany(x => x.Permissions).WithMany(x => x.Roles);

        builder.HasData(
            new Role 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "Registered", 
                    IsDefault = true,
                    IsDeletable = false,
                    Permissions = 
                        [
                            new Permission {Id = Guid.Parse("EF1402BB-6B8A-41F6-86A5-4548B0DD55C0")}
                        ] 
                },
            new Role
            {
                Id = Guid.NewGuid(), 
                Name = "Administrator",
                IsDefault = false,
                IsDeletable = false,
                Permissions = 
                    [
                        new Permission{ Id = Guid.Parse("EF1402BB-6B8A-41F6-86A5-4548B0DD55C0")},
                        new Permission{ Id = Guid.Parse("97A792D7-D09A-4E6E-954D-1F34071D8041")}
                    ]
            });
    }
}
