using Application.Abstractions.Cryptography;
using Application.Abstractions.EmailSender;
using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Repositories;

namespace Application.Users.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IPasswordHashChecker passwordHashChecker,
    IEmailSender emailSender)    
        : ICommandHandler<ChangePasswordCommand>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
            disableQuerySpliting: true,
            cancellationToken: cancellationToken);

        if (user is null)
        {
            return Result.Failure(DomainErrors.User.NotFound);
        }
        
        var isValidPassword = passwordHashChecker.HashesMatch(user.PasswordHash, request.CurrentPassword);

        if (!isValidPassword)
        {
            return Result.Failure(DomainErrors.User.InvalidCredentials);
        }

        var hashedPassword = passwordHasher.HashPassword(request.NewPassword);

        await userRepository.ExecuteUpdateAsync(
            predicate: x => x.Id == user.Id,
            property: set => set.SetProperty(x => x.PasswordHash, hashedPassword), 
            cancellationToken);

        await emailSender.SendAsync(user.Email!, "Password was changed", "Your password was successfuly changed!");

        return Result.Success();
    }
}
