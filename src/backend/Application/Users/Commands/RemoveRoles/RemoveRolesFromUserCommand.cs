using Application.Abstractions.Messaging;

namespace Application.Users.Commands.RemoveRoles;

public sealed record RemoveRolesFromUserCommand(Guid UserId, List<Guid> RoleIds) : ICommand;
