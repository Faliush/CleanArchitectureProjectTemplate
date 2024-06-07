using Application.Abstractions.Messaging;
using Domain.Enums;

namespace Application.Roles.Queries.GetAllPermissions;

public sealed record GetAllPermissionsQuery : IQuery<List<Permissions>>;
