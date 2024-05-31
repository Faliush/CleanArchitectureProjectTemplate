using Domain.Core.Abstractions;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public sealed class User : IdentityUser<Guid>, IAuditable
{
    public void SetRefreshToken(string refreshToken)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
    }

    public Result VerifyRefreshToken(string refreshToken)
    {
        if (!string.IsNullOrWhiteSpace(refreshToken) ||
            RefreshToken != refreshToken ||
            RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return Result.Failure(DomainErrors.User.InvalidRefreshToken);
        }

        return Result.Success();
    }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";

    public DateTime CreatedOnUtc { get; }

    public DateTime? ModifiedOnUtc { get; }

    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiryTime { get; set; }
}
