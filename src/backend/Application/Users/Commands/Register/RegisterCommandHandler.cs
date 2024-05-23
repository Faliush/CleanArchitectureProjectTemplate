using Application.Abstractions.Authentication.Jwt;
using Application.Abstractions.Cryptography;
using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Repositories.Contracts;
using Infrastructure.UnitOfWork;

namespace Application.Users.Commands.Register;

internal sealed class RegisterCommandHandler(
    IUnitOfWork unitOfWork,
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider)
        : ICommandHandler<RegisterCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var firstNameResult = FirstName.Create(request.FirstName);
        var lastNameResult = LastName.Create(request.LastName);
        var emailResult = Email.Create(request.Email);
        var passwrodResult = Password.Create(request.Password);

        var firstFaliureOrSuccess = Result.FirstFailureOrSuccess(firstNameResult, lastNameResult, emailResult, passwrodResult);

        if (firstFaliureOrSuccess.IsFailure)
        {
            return Result.Failure<string>(firstFaliureOrSuccess.Error);
        }

        if (!await userRepository.IsEmailUniqueAsync(emailResult.Value, cancellationToken))
        {
            return Result.Failure<string>(DomainErrors.User.InvalidCredentials);
        }

        if (!await roleRepository.Exists(request.RoleId, cancellationToken))
        {
            return Result.Failure<string>(DomainErrors.Role.NotFound);
        }

        var hasherPassword = passwordHasher.HashPassword(passwrodResult.Value);

        var user = User.Create(firstNameResult.Value, lastNameResult.Value, emailResult.Value, hasherPassword, request.RoleId);

        await userRepository.InsertAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        string token = await jwtProvider.Generate(user);

        return Result.Success(token);
    }
}
