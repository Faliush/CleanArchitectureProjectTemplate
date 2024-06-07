using Application.Abstractions.Messaging;

namespace Application.Users.Commands.Update;

public sealed record UpdateUserCommand(Guid Id, string FirstName, string LastName) : ICommand;
