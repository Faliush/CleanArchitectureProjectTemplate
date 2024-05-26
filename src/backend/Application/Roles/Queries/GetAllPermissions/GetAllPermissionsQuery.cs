using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;

namespace Application.Roles.Queries.GetAllPermissions;

public sealed record GetAllPermissionsQuery : IQuery<Result>;
