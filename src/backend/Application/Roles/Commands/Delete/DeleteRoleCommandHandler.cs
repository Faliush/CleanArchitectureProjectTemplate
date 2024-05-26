using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Infrastructure.Repositories.Contracts;
using Infrastructure.UnitOfWork;

namespace Application.Roles.Commands.Delete;

internal sealed class DeleteRoleCommandHandler(
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) 
    : ICommandHandler<DeleteRoleCommand, Result>
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

        roleRepository.Delete(role);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
