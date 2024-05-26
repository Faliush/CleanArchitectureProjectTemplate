using Application.Roles.Queries.GetAllPermissions;

namespace Application.Roles.Queries.GetAll;

public sealed class RoleResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<PermissionResponse> Permissions { get; set; } = [];
}
