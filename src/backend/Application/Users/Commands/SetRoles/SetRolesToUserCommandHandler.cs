using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Repositories;
using Domain.UnitOfWork;

namespace Application.Users.Commands.SetRoles;

internal sealed class SetRolesToUserCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork) 
    : ICommandHandler<SetRolesToUserCommand>
{
    public async Task<Result> Handle(SetRolesToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.UserId,
            disableQuerySpliting: true,
            cancellationToken: cancellationToken);

        if(user is null)
        {
            return Result.Failure(DomainErrors.User.NotFound);
        }

        var roles = await roleRepository.GetAllAsync(
            predicate: x => request.RoleIds.Contains(x.Id),
            disableQuerySpliting: true,
            cancellationToken: cancellationToken);

        if(roles.Count == 0)
        {
            return Result.Failure(DomainErrors.Role.NotFound);
        }

        await userRepository.ExecuteUpdateAsync(
            predicate: x => x.Id == user.Id,
            property: set => set.SetProperty(x => x.Roles, roles),
            cancellationToken: cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
