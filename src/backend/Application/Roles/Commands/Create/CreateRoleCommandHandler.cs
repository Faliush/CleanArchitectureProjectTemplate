using Application.Abstractions.Messaging;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Repositories.Contracts;
using Infrastructure.UnitOfWork;

namespace Application.Roles.Commands.Create;

internal sealed class CreateRoleCommandHandler(
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateRoleCommand, Result>
{
    public async Task<Result> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var nameResult = Name.Create(request.Name);

        if (nameResult.IsFailure)
        {
            return Result.Failure(nameResult.Error);
        }

        var role = Role.Create(nameResult.Value, request.Permissions);

        await roleRepository.InsertAsync(role, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
