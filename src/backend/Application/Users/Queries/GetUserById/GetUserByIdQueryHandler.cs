using Application.Abstractions.Messaging;
using Application.Roles.Queries.GetAll;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetUserById;

internal sealed class GetUserByIdQueryHandler(
    IUserRepository userRepository)
        : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetFirstOrDefaultAsync(
            disableTracking: true,
            include: i => i.Include(x => x.Roles),
            selector: x => new UserResponse(x.Id, x.FullName, x.Email, x.Roles
                .Select(r => new RoleResponse(r.Id, r.Name))), 
            cancellationToken: cancellationToken);

        return user ?? Result.Failure<UserResponse>(DomainErrors.User.NotFound);
    }
}
