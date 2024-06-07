using Application.Abstractions.Messaging;

namespace Application.Users.Commands.AddRoles;

public sealed record AddRolesToUserCommand(Guid UserId, List<Guid> RoleIds) : ICommand;
