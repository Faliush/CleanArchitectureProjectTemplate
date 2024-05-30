namespace Application.Users.Commands.AddRoles;

public sealed record AddRolesToUserRequest(List<Guid> RoleIds);
