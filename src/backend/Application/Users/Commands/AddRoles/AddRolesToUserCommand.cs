using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;

namespace Application.Users.Commands.AddRoles;

public sealed record AddRolesToUserCommand(Guid UserId, List<Guid> RoleIds) : ICommand<Result>;
