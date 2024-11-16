using Domain.Core.Abstractions;
using Domain.Core.Errors;
using Domain.Core.Primitives;
using Domain.Core.Primitives.Result;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public sealed class User : Entity, IAuditable, ISoftDeletable
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
    
    public string Email { get; set; }
    
    public string PasswordHash { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiryTime { get; set; }
    
    public DateTime CreatedOnUtc { get; }

    public DateTime? ModifiedOnUtc { get; }
    
    public DateTime? DeletedOnUtc { get; }
    public bool Deleted { get; }

    public IList<Role> Roles { get; set; } = [];
}
