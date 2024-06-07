using Application.Abstractions.Messaging;

namespace Application.Roles.Commands.Delete;

public sealed record DeleteRoleCommand(Guid Id) : ICommand;
