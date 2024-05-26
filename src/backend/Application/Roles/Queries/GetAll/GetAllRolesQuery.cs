using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;

namespace Application.Roles.Queries.GetAll;

public sealed record GetAllRolesQuery : IQuery<Result>;
