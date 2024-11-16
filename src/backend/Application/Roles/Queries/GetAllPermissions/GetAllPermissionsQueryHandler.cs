using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;
using Domain.Enums;
using Domain.Repositories;

namespace Application.Roles.Queries.GetAllPermissions;

internal sealed class GetAllPermissionsQueryHandler(IPermissionRepository permissionRepository)
    : IQueryHandler<GetAllPermissionsQuery, IEnumerable<PermissionResponse>>
{
    public async Task<Result<IEnumerable<PermissionResponse>>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
    {
        var permissions = await permissionRepository.GetAllAsync(
            selector: x => new PermissionResponse(x.Id, x.Name),
            cancellationToken: cancellationToken);

        return permissions.ToList();
    }
}
