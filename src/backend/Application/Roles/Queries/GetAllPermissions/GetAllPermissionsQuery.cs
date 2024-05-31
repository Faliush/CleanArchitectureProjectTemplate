using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;
using Domain.Enums;

namespace Application.Roles.Queries.GetAllPermissions;

public sealed record GetAllPermissionsQuery : IQuery<Result<List<Permissions>>>;
