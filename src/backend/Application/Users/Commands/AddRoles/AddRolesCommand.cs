using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;

namespace Application.Users.Commands.AddRoles;

public sealed record AddRolesCommand : ICommand<Result>;
