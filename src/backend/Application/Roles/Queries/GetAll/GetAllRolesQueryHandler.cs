using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;
using Infrastructure.Repositories.Contracts;

namespace Application.Roles.Queries.GetAll;

internal sealed class GetAllRolesQueryHandler(IRoleRepository roleRepository)
    : IQueryHandler<GetAllRolesQuery, Result>
{
    public async Task<Result> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleRepository.GetAllAsync(
            disableTracking: true,
            disableQuerySpliting: false,
            selector: s => new RoleResponse
            {
                Id = s.Id,
                Name = s.Name,
                Permissions = s.Permissions!.ToList()
            },
            cancellationToken: cancellationToken);

        return Result.Success(roles);
    }
}
