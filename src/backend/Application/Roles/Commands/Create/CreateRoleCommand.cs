using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;

namespace Application.Roles.Commands.Create;

public sealed record CreateRoleCommand(string Name) : ICommand<Result>;
