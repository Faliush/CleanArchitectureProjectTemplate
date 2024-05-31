using Application.Abstractions.Authentication.Jwt;
using Application.Abstractions.EmailSender;
using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Authentication.Commands.GoogleSignIn;

internal sealed class GoogleSignInCommandHandler(
    UserManager<User> userManager,
    IJwtProvider jwtProvider,
    IEmailSender emailSender)
    : ICommandHandler<GoogleSignInCommand, Result>
{
    public async Task<Result> Handle(GoogleSignInCommand request, CancellationToken cancellationToken)
    {
        var payload = await jwtProvider.VerifyGoogleTokenAsync(request.IdToken);

        if(payload is null)
        {
            return Result.Failure<AuthenticatedResponse>(DomainErrors.User.InvalidExternalAuthentication);
        }

        var info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);

        var user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

        user ??= await userManager.FindByEmailAsync(payload.Email);

        if(user is null)
        {
            user = new User 
            { 
                FirstName = payload.Name, 
                LastName = payload.FamilyName,
                Email = payload.Email,
            };

            await userManager.AddToRoleAsync(user, "Registered");
        }
        
        await userManager.AddLoginAsync(user, info);

        string accesToken = await jwtProvider.GenerateAccessTokenAsync(user);
        string refreshToken = jwtProvider.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken);

        await userManager.UpdateAsync(user);

        await emailSender.SendAsync(
            user.Email!,
            "Successfully Registered!",
            "Thank you for registering with us. We're excited to have you on board!");

        return Result.Success(new AuthenticatedResponse(accesToken, refreshToken));
    }
}
