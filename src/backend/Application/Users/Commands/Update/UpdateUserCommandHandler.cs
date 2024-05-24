using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.ValueObjects;
using Infrastructure.Repositories.Contracts;
using Infrastructure.UnitOfWork;

namespace Application.Users.Commands.Update;

internal sealed class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) 
    : ICommandHandler<UpdateUserCommand, Result>
{
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var firstNameResult = FirstName.Create(request.FirstName);
        var lastNameResult = LastName.Create(request.LastName);

        var firstFaliureOrSuccess = Result.FirstFailureOrSuccess(firstNameResult, lastNameResult);

        if (firstFaliureOrSuccess.IsFailure)
        {
            return Result.Failure(firstFaliureOrSuccess.Error);
        }

        var user = await userRepository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
            disableTracking: false,
            disableQuerySpliting: true,
            cancellationToken: cancellationToken);

        if (user is null) 
        {
            return Result.Failure(DomainErrors.User.NotFound);
        }

        user.ChangeName(firstNameResult.Value, lastNameResult.Value);

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
