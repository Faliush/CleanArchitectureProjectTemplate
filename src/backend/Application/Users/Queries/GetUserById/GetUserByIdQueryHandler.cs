using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Infrastructure.Repositories.Contracts;

namespace Application.Users.Queries.GetUserById;

internal sealed class GetUserByIdQueryHandler(
    IUserRepository userRepository)
        : IQueryHandler<GetUserByIdQuery, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
            disableTracking: true,
            disableQuerySpliting: false,
            selector: u => new UserResponse(u.Id, u.FirstName, u.LastName, u.FullName, u.Email, u.CreatedOnUtc),
            cancellationToken: cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponse>(DomainErrors.User.NotFound);
        }

        return user;
    }
}
