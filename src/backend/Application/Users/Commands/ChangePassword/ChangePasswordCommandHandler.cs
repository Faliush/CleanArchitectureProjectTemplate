using Application.Abstractions.Cryptography;
using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.ValueObjects;
using Infrastructure.Repositories.Contracts;
using Infrastructure.UnitOfWork;

namespace Application.Users.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher)    
        : ICommandHandler<ChangePasswordCommand, Result>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var passwordResult = Password.Create(request.Password);

        if (passwordResult.IsFailure)
        {
            return Result.Failure(DomainErrors.User.InvalidCredentials);
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

        var passwordHash = passwordHasher.HashPassword(passwordResult.Value);

        user.ChangePassword(passwordHash);

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
