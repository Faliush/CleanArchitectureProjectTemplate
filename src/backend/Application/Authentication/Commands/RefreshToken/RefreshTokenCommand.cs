using Application.Abstractions.Messaging;

namespace Application.Authentication.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string AccessToken, string RefreshToken) 
    : ICommand<AuthenticatedResponse>;
