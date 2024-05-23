using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;

namespace Application.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid Id) : IQuery<Result<UserResponse>>;
