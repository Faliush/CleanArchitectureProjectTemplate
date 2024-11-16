using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Repositories;
using Domain.UnitOfWork;

namespace Application.Users.Commands.Update;

internal sealed class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) 
    : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
            disableQuerySpliting: true,
            cancellationToken: cancellationToken);

        if (user is null) 
        {
            return Result.Failure(DomainErrors.User.NotFound);
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        userRepository.Update(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
