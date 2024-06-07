using Application.Abstractions.Messaging;

namespace Application.Authentication.Commands.GoogleSignIn;

public sealed record GoogleSignInCommand(string Provider, string IdToken) : ICommand<AuthenticatedResponse>;
