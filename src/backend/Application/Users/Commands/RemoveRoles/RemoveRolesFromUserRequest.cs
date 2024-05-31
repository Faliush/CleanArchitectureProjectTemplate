namespace Application.Users.Commands.RemoveRoles;

public sealed record RemoveRolesFromUserRequest(List<Guid> RoleIds);
