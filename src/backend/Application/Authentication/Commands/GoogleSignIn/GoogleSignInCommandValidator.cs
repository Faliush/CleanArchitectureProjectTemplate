using FluentValidation;

namespace Application.Authentication.Commands.GoogleSignIn;

public sealed class GoogleSignInCommandValidator : AbstractValidator<GoogleSignInCommand>
{
    public GoogleSignInCommandValidator()
    {
        RuleFor(x => x.IdToken).NotEmpty();

        RuleFor(x => x.Provider).NotEmpty();
    }
}
