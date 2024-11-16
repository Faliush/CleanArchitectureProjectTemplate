using Application.Roles.Queries.GetAll;

namespace Application.Users.Queries.GetUserById;

public sealed record UserResponse(
    Guid Id,
    string FullName,
    string Email,
    IEnumerable<RoleResponse> Roles);
