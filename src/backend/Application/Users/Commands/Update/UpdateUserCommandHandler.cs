using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Commands.Update;

internal sealed class UpdateUserCommandHandler(
    UserManager<User> userManager) 
    : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());

        if (user is null) 
        {
            return Result.Failure(DomainErrors.User.NotFound);
        }

        var updatedUser = new User 
        { 
            FirstName = request.FirstName,
            LastName = request.LastName,
            Id = user.Id, 
            Email = user.Email,
            RefreshToken = user.RefreshToken,
            RefreshTokenExpiryTime = user.RefreshTokenExpiryTime,
        };

        await userManager.UpdateAsync(updatedUser);

        return Result.Success();
    }
}
