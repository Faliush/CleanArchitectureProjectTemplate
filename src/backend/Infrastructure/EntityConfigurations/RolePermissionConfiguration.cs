using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission = Domain.Enums.Permission;

namespace Infrastructure.EntityConfigurations;

internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        builder.HasData(
            Create(Role.Registered, Permission.ReadUsers), 
            Create(Role.Administrator, Permission.ManageUsers),
            Create(Role.Administrator, Permission.ReadUsers));
    }

    private static RolePermission Create(Role role, Permission permission)
    {
        return new RolePermission { RoleId = role.Id, PermissionId = (int)permission };
    }
}
