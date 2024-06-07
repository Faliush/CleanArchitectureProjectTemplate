using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;
using Domain.Enums;

namespace Application.Roles.Queries.GetAllPermissions;

internal sealed class GetAllPermissionsQueryHandler
    : IQueryHandler<GetAllPermissionsQuery, List<Permissions>>
{
    public Task<Result<List<Permissions>>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions = Enum.GetValues<Permissions>().ToList();

        return Task.FromResult(Result.Success(permissions));
    }
}
