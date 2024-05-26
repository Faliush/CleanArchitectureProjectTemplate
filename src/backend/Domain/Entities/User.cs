using Domain.Core.Abstractions;
using Domain.Core.Errors;
using Domain.Core.Primitives;
using Domain.Core.Primitives.Result;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class User : AggregateRoot, IAuditable
{
    private User(FirstName firstName, LastName lastName, Email email, string passwordHash)
        : base(Guid.NewGuid())
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
    }

    private User() { }

    public static User Create(FirstName firstName, LastName lastName, Email email, string passwordHash)
    {
        var user = new User(firstName, lastName, email, passwordHash);

        // this can be domain event

        return user;
    }

    public Result ChangePassword(string passwordHash)
    {
        PasswordHash = passwordHash;

        // domain event

        return Result.Success();
    }

    public void ChangeName(FirstName firstName, LastName lastName) 
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public Result AddToRoles(params Role[] roles)
    {
        foreach (var role in roles)
        {
            if (!Roles.Contains(role))
            {
                Roles.Add(role);
            }
        }

        return Result.Success();
    }

    public Result RemoveRoles(params Role[] roles)
    {
        foreach(var role in roles)
        {
            if (!Roles.Contains(role))
            {
                return Result.Failure(DomainErrors.Role.NotFound);
            }

            Roles.Remove(role);
        }

        return Result.Success();
    }

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

    public FirstName FirstName { get; private set; }

    public LastName LastName { get; private set; }

    public Email Email { get; private set; }

    public string PasswordHash { get; private set; }

    public string FullName => $"{FirstName} {LastName}";

    public DateTime CreatedOnUtc { get; }

    public DateTime? ModifiedOnUtc { get; }

    public string? RefreshToken { get; private set; }

    public DateTime RefreshTokenExpiryTime { get; private set; }

    public ICollection<Role> Roles { get; } = [];
}
