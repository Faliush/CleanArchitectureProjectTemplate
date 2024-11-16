using Application.Abstractions.Messaging;

namespace Application.Users.Commands.SetRoles;

public sealed record SetRolesToUserCommand(Guid UserId, List<Guid> RoleIds) : ICommand;
