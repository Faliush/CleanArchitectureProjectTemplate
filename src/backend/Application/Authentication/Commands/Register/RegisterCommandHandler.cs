using Application.Abstractions.Authentication.Jwt;
using Application.Abstractions.EmailSender;
using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Authentication.Commands.Register;

internal sealed class RegisterCommandHandler(
    UserManager<User> userManager,
    IJwtProvider jwtProvider,
    IEmailSender emailSender)
        : ICommandHandler<RegisterCommand, AuthenticatedResponse>
{
    public async Task<Result<AuthenticatedResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var isEmailUnique = await userManager.FindByEmailAsync(request.Email) is null;

        if (!isEmailUnique)
        {
            return Result.Failure<AuthenticatedResponse>(DomainErrors.User.InvalidCredentials);
        }

        var user = new User { FirstName = request.FirstName, LastName = request.LastName, Email = request.Email };

        var userCreatedResult = await userManager.CreateAsync(user, request.Password);

        if (!userCreatedResult.Succeeded)
        {
            return Result.Failure<AuthenticatedResponse>(DomainErrors.User.InvalidCredentials);
        }
        await userManager.AddToRoleAsync(user, "Registered");

        string accesToken = await jwtProvider.GenerateAccessTokenAsync(user);
        string refreshToken = jwtProvider.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken);

        await userManager.UpdateAsync(user);

        await emailSender.SendAsync(
            user.Email,
            "Successfully Registered!",
            "Thank you for registering with us. We're excited to have you on board!");

        return Result.Success(new AuthenticatedResponse(accesToken, refreshToken));
    }
}
