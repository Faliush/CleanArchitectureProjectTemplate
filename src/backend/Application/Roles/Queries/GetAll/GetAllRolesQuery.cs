using Application.Abstractions.Messaging;

namespace Application.Roles.Queries.GetAll;

public sealed record GetAllRolesQuery : IQuery<List<RoleResponse>>;
