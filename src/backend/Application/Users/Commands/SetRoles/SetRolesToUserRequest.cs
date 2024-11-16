namespace Application.Users.Commands.SetRoles;

public sealed record SetRolesToUserRequest(List<Guid> RoleIds);
