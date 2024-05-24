using FluentValidation;

namespace Application.Authentication.Commands.RefreshToken;

public sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.AccessToken).NotEmpty();

        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}
