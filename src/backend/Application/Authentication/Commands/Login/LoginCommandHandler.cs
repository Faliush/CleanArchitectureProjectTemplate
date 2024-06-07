using Application.Abstractions.Authentication.Jwt;
using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Authentication.Commands.Login;

internal sealed class LoginCommandHandler(
    UserManager<User> userManager,
    IJwtProvider jwtProvider)
        : ICommandHandler<LoginCommand, AuthenticatedResponse>
{
    public async Task<Result<AuthenticatedResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Result.Failure<AuthenticatedResponse>(DomainErrors.User.InvalidCredentials);
        }

        var isValidPassword = await userManager.CheckPasswordAsync(user, request.Password);

        if (!isValidPassword)
        {
            return Result.Failure<AuthenticatedResponse>(DomainErrors.User.InvalidCredentials);
        }

        var accessToken = await jwtProvider.GenerateAccessTokenAsync(user);
        var refreshToken = jwtProvider.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken);

        return Result.Success(new AuthenticatedResponse(accessToken, refreshToken));
    }
}
