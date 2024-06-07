using Application.Abstractions.Messaging;

namespace Application.Authentication.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<AuthenticatedResponse>;
