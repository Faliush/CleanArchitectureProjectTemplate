using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;
using Infrastructure.Repositories.Contracts;

namespace Application.Roles.Queries.GetAllPermissions;

internal sealed class GetAllPermissionsQueryHandler(
    IPermissionRepository permissionRepository)
    : IQueryHandler<GetAllPermissionsQuery, Result>
{
    public async Task<Result> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions = await permissionRepository.GetAllAsync(
            disableTracking: true,
            disableQuerySpliting: true,
            selector: s => new PermissionResponse
            {
                Id = s.Id,
                Name = s.Name,
            },
            cancellationToken: cancellationToken);

        return Result.Success(permissions);
    }
}
