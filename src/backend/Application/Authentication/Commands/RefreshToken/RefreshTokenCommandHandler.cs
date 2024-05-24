﻿using Application.Abstractions.Authentication.Jwt;
using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Infrastructure.Repositories.Contracts;
using Infrastructure.UnitOfWork;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Authentication.Commands.RefreshToken;

internal sealed class RefreshTokenCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IJwtProvider jwtProvider) 
    : ICommandHandler<RefreshTokenCommand, Result<AuthenticatedResponse>>
{
    public async Task<Result<AuthenticatedResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = jwtProvider.GetPrincipalFromExpiredToken(request.AccessToken);
        
        var userId = principal.Claims
            .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

        if (!Guid.TryParse(userId, out Guid parsedUserId))
        {
            return Result.Failure<AuthenticatedResponse>(DomainErrors.User.InvalidClaims);
        }

        var user = await userRepository.GetFirstOrDefaultAsync(
            predicate: x => x.Id == parsedUserId,
            disableTracking: false,
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

        string accesToken = await jwtProvider.GenerateAccessTokenAsync(user);
        string refreshToken = jwtProvider.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken);

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new AuthenticatedResponse(accesToken, refreshToken));
    }
}
