using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;

namespace Application.Authentication.Commands.RefreshToken;

public sealed record RefreshTokenCommand(string AccessToken, string RefreshToken) 
    : ICommand<Result<AuthenticatedResponse>>;
