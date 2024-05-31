using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;

namespace Application.Users.Commands.RemoveRoles;

public sealed record RemoveRolesFromUserCommand(Guid UserId, List<Guid> RoleIds) : ICommand<Result>;
