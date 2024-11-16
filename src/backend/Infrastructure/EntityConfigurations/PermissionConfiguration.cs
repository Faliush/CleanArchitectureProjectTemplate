using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasData(
            new Permission { Id = Guid.Parse("EF1402BB-6B8A-41F6-86A5-4548B0DD55C0"), Name = nameof(Permissions.Default) },
            new Permission { Id = Guid.Parse("97A792D7-D09A-4E6E-954D-1F34071D8041"), Name = nameof(Permissions.FullAccess) });
    }
}