using Application.Abstractions.Authentication.Jwt;
using Application.Abstractions.Cryptography;
using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Repositories.Contracts;
using Infrastructure.UnitOfWork;

namespace Application.Authentication.Commands.Register;

internal sealed class RegisterCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider)
        : ICommandHandler<RegisterCommand, Result<AuthenticatedResponse>>
{
    public async Task<Result<AuthenticatedResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var firstNameResult = FirstName.Create(request.FirstName);
        var lastNameResult = LastName.Create(request.LastName);
        var emailResult = Email.Create(request.Email);
        var passwrodResult = Password.Create(request.Password);

        var firstFaliureOrSuccess = Result.FirstFailureOrSuccess(firstNameResult, lastNameResult, emailResult, passwrodResult);

        if (firstFaliureOrSuccess.IsFailure)
        {
            return Result.Failure<AuthenticatedResponse>(firstFaliureOrSuccess.Error);
        }

        if (!await userRepository.IsEmailUniqueAsync(emailResult.Value, cancellationToken))
        {
            return Result.Failure<AuthenticatedResponse>(DomainErrors.User.InvalidCredentials);
        }

        var hasherPassword = passwordHasher.HashPassword(passwrodResult.Value);

        var user = User.Create(firstNameResult.Value, lastNameResult.Value, emailResult.Value, hasherPassword);

        user.AddToRoles(Role.Registered);

        string accesToken = await jwtProvider.GenerateAccessTokenAsync(user);
        string refreshToken = jwtProvider.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken);
        
        await userRepository.InsertAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new AuthenticatedResponse(accesToken, refreshToken));
    }
}
