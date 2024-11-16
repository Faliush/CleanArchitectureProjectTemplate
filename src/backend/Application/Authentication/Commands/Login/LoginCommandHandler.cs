using Application.Abstractions.Authentication.Jwt;
using Application.Abstractions.Cryptography;
using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Application.Authentication.Commands.Login;

internal sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHashChecker passwordHashChecker,
    IJwtProvider jwtProvider)
        : ICommandHandler<LoginCommand, AuthenticatedResponse>
{
    public async Task<Result<AuthenticatedResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
        {
            return Result.Failure<AuthenticatedResponse>(DomainErrors.User.InvalidCredentials);
        }

        var isValidPassword = passwordHashChecker.HashesMatch(user.PasswordHash, request.Password);

        if (!isValidPassword)
        {
            return Result.Failure<AuthenticatedResponse>(DomainErrors.User.InvalidCredentials);
        }

        var accessToken = await jwtProvider.GenerateAccessTokenAsync(user);
        var refreshToken = jwtProvider.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken);

        userRepository.Update(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new AuthenticatedResponse(accessToken, refreshToken));
    }
}
