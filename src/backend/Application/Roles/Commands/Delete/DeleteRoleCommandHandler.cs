using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace Application.Roles.Commands.Delete;

internal sealed class DeleteRoleCommandHandler(
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) 
    : ICommandHandler<DeleteRoleCommand>
{
    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
            disableQuerySpliting: true,
            cancellationToken: cancellationToken);

        if(role is null)
        {
            return Result.Failure(DomainErrors.Role.NotFound);
        }

        await roleRepository.ExecuteDeleteAsync(x => x.Id == role.Id, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
