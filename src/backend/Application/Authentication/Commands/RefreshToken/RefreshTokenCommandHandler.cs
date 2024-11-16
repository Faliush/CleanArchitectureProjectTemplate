using Application.Abstractions.Authentication.Jwt;
using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Repositories;
using Domain.UnitOfWork;

namespace Application.Authentication.Commands.RefreshToken;

internal sealed class RefreshTokenCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IJwtProvider jwtProvider) 
    : ICommandHandler<RefreshTokenCommand, AuthenticatedResponse>
{
    public async Task<Result<AuthenticatedResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = jwtProvider.GetPrincipalFromExpiredToken(request.AccessToken);
        
        var userId = principal.Claims
            .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub || x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userId, out var parsedUserId))
        {
            return Result.Failure<AuthenticatedResponse>(DomainErrors.User.InvalidClaims);
        }

        var user = await userRepository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == parsedUserId,
            disableQuerySpliting: true,
            cancellationToken: cancellationToken);

        if(user is null)
        {
            return Result.Failure<AuthenticatedResponse>(DomainErrors.User.NotFound);
        }

        var verified = user.VerifyRefreshToken(request.RefreshToken);

        if (verified.IsFailure)
        {
            return Result.Failure<AuthenticatedResponse>(DomainErrors.User.InvalidRefreshToken);
        }

        var accesToken = await jwtProvider.GenerateAccessTokenAsync(user);
        var refreshToken = jwtProvider.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken);

        userRepository.Update(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new AuthenticatedResponse(accesToken, refreshToken));
    }
}
