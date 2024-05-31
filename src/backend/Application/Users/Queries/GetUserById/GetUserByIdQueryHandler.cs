using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Users.Queries.GetUserById;

internal sealed class GetUserByIdQueryHandler(
    UserManager<User> userManager)
        : IQueryHandler<GetUserByIdQuery, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());

        if (user is null)
        {
            return Result.Failure<UserResponse>(DomainErrors.User.NotFound);
        }

        var result = new UserResponse(
            user.Id, 
            user.FirstName,
            user.LastName,
            user.FullName,
            user.Email!, 
            user.CreatedOnUtc);

        return result;
    }
}
