using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(x => x.Id);

        builder.OwnsOne(role => role.Name, nameBuilder =>
        {
            nameBuilder.WithOwner();

            nameBuilder.Property(x => x.Value)
                .HasColumnName(nameof(Name))
                .HasMaxLength(Name.MaxLength)
                .IsRequired();
        });

        builder.HasMany(x => x.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>();

        builder.HasMany(x => x.Users)
            .WithMany();

        builder.HasData(new {Role.Registered, Role.Administrator}); 
    }
}
