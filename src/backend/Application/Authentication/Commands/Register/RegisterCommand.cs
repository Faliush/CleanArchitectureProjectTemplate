using Application.Abstractions.Messaging;

namespace Application.Authentication.Commands.Register;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password)
        : ICommand<AuthenticatedResponse>;
