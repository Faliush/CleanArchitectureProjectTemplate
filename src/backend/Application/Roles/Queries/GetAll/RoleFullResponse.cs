using Application.Roles.Queries.GetAllPermissions;

namespace Application.Roles.Queries.GetAll;

public sealed record RoleResponse(Guid Id, string Name);
public sealed record RoleFullResponse(Guid Id, string Name, IEnumerable<PermissionResponse> Permissions);

