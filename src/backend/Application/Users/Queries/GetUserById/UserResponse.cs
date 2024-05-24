namespace Application.Users.Queries.GetUserById;

public sealed record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    DateTime CreatedOnUtc);
