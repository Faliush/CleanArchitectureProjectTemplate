using Domain.Entities;
using System.Security.Claims;
namespace Application.Abstractions.Authentication.Jwt;

public interface IJwtProvider
{
    Task<string> GenerateAccessTokenAsync(User user);

    string GenerateRefreshToken();

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
