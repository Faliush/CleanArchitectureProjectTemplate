using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(x => x.Id);

        var permissions = Enum.GetValues<Domain.Enums.Permission>()
                              .Select(p => new Permission 
                              { 
                                  Id = (int)p, 
                                  Name = nameof(p) 
                              });

        builder.HasData(permissions);
    }
}
