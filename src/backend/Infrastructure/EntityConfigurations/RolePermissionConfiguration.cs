using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permissions = Domain.Enums.Permissions;

namespace Infrastructure.EntityConfigurations;

internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        builder.HasData(
            Create(Role.Registered, Permissions.User), 
            Create(Role.Administrator, Permissions.All));
    }

    private static RolePermission Create(Role role, Permissions permission)
    {
        return new RolePermission { RoleId = role.Id, PermissionId = (int)permission };
    }
}
