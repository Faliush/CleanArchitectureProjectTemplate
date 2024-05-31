using Application.Abstractions.EmailSender;
using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands.ChangePassword;

internal sealed class ChangePasswordCommandHandler(
    UserManager<User> userManager,
    IEmailSender emailSender)    
        : ICommandHandler<ChangePasswordCommand, Result>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());

        if (user is null)
        {
            return Result.Failure(DomainErrors.User.NotFound);
        }

        var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            return Result.Failure(DomainErrors.User.CannotChangePassword);
        }

        await userManager.UpdateAsync(user);

        await emailSender.SendAsync(user.Email!, "Password was changed", "Your password was successfuly changed!");

        return Result.Success();
    }
}
