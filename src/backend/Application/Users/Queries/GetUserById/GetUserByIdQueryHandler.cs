using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Infrastructure.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.GetUserById;

internal sealed class GetUserByIdQueryHandler(
    IUserRepository userRepository)
        : IQueryHandler<GetUserByIdQuery, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
            include: i => i.Include(x => x.Role),
            disableTracking: true,
            disableQuerySpliting: false,
            selector: u => new UserResponse(u.Id, u.FirstName, u.LastName, u.FullName, u.Email, u.CreatedOnUtc, u.Role.Name),
            cancellationToken: cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponse>(DomainErrors.User.NotFound);
        }

        return user;
    }
}
