using Application.Abstractions.Authentication.Jwt;
using Application.Abstractions.Cryptography;
using Application.Abstractions.EmailSender;
using Application.Abstractions.Messaging;
using Domain.Core.Errors;
using Domain.Core.Primitives.Result;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;

namespace Application.Authentication.Commands.Register;

internal sealed class RegisterCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IJwtProvider jwtProvider,
    IEmailSender emailSender)
        : ICommandHandler<RegisterCommand, AuthenticatedResponse>
{
    public async Task<Result<AuthenticatedResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var isEmailUnique = await userRepository.IsEmailUniqueAsync(request.Email, cancellationToken);

        if (!isEmailUnique)
        {
            return Result.Failure<AuthenticatedResponse>(DomainErrors.User.InvalidCredentials);
        }

        var hashedPassword = passwordHasher.HashPassword(request.Password);
        
        var roles = await roleRepository.GetAllAsync(
            predicate: x => x.IsDefault == true,
            disableQuerySpliting: true, 
            cancellationToken: cancellationToken);
        
        var user = new User
        {
            FirstName = request.FirstName, 
            LastName = request.LastName, 
            Email = request.Email,
            PasswordHash = hashedPassword,
            Roles = roles
        };

        var accesToken = await jwtProvider.GenerateAccessTokenAsync(user);
        var refreshToken = jwtProvider.GenerateRefreshToken();

        user.SetRefreshToken(refreshToken);

        await userRepository.InsertAsync(user, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await emailSender.SendAsync(
            user.Email,
            "Successfully Registered!",
            "Thank you for registering with us. We're excited to have you on board!");

        return Result.Success(new AuthenticatedResponse(accesToken, refreshToken));
    }
}
