using Application.Abstractions.Authentication.Jwt;
using Application.Abstractions.Cryptography;
using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.ValueObjects;
using Infrastructure.Repositories.Contracts;

namespace Application.Users.Login;

internal sealed class LoginCommandHandler(
    IUserRepository userRepository, 
    IJwtProvider jwtProvider,
    IPasswordHashChecker passwordHashChecker) 
        : ICommandHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = Email.Create(request.Email);

        if (email.IsFailure)
        {
            return Result.Failure<string>(DomainErrors.User.InvalidCredentials);
        }

        var user = await userRepository.GetByEmailAsync(email.Value, cancellationToken);
    
        if(user is null)
        {
            return Result.Failure<string>(DomainErrors.User.InvalidCredentials);
        }

        var validPassword = passwordHashChecker.HashesMatch(user.PasswordHash, request.Password);

        if (!validPassword)
        {
            return Result.Failure<string>(DomainErrors.User.InvalidCredentials);
        }

        var token = await jwtProvider.Generate(user);

        return Result.Success(token);
    }
}
