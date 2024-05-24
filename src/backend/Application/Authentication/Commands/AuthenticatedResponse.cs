namespace Application.Authentication.Commands;

public sealed record AuthenticatedResponse(string? AccessToken, string? RefreshToken);
