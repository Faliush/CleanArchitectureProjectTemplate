using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;
using Domain.Enums;
using Infrastructure.Repositories.Contracts;

namespace Application.Roles.Queries.GetAllPermissions;

internal sealed class GetAllPermissionsQueryHandler
    : IQueryHandler<GetAllPermissionsQuery, Result>
{
    public async Task<Result> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions = Enum.GetValues<Permissions>();

        return Result.Success(permissions);
    }
}
