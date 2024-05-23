using Domain.Core.Abstractions;
using Domain.Core.Primitives;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class User : AggregateRoot, IAuditable
{
    private User(FirstName firstName, LastName lastName, Email email, string passwordHash, int roleId)
        : base(Guid.NewGuid())
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        RoleId = roleId;

    }

    private User() { }

    public static User Create(FirstName firstName, LastName lastName, Email email, string passwordHash, int RoleId)
    {
        var user = new User(firstName, lastName, email, passwordHash, RoleId);

        // this can be domain event

        return user;
    }

    public FirstName FirstName { get; private set; }

    public LastName LastName { get; private set; }

    public Email Email { get; private set; }

    public string PasswordHash { get; private set; }

    public string FullName => $"{FirstName} {LastName}";

    public DateTime CreatedOnUtc { get; }

    public DateTime? ModifiedOnUtc { get; }

    public int RoleId { get; private set; }
    public Role Role { get; set; }
}
