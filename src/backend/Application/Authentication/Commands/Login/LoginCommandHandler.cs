using Application.Abstractions.Authentication.Jwt;
using Application.Abstractions.Cryptography;
using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.ValueObjects;
using Infrastructure.Repositories.Contracts;

namespace Application.Authentication.Commands.Login;

internal sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtProvider jwtProvider,
    IPasswordHashChecker passwordHashChecker)
        : ICommandHandler<LoginCommand, Result<AuthenticatedResponse>>
{
    public async Task<Result<AuthenticatedResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = Email.Create(request.Email);

        if (email.IsFailure)
        {
            return Result.Failure<AuthenticatedResponse>(DomainErrors.User.InvalidCredentials);
        }

        var user = await userRepository.GetByEmailAsync(email.Value, cancellationToken);

        if (user is null)
        {
            return Result.Failure<AuthenticatedResponse>(DomainErrors.User.InvalidCredentials);
        }

        var validPassword = passwordHashChecker.HashesMatch(user.PasswordHash, request.Password);

        if (!validPassword)
        {
            return Result.Failure<AuthenticatedResponse>(DomainErrors.User.InvalidCredentials);
        }

        var accessToken = await jwtProvider.GenerateAccessTokenAsync(user);
        var refreshToken = jwtProvider.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken);

        return Result.Success(new AuthenticatedResponse(accessToken, refreshToken));
    }
}
