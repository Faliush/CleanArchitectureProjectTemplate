﻿using FluentValidation;

namespace Application.Users.Commands.ChangePassword;

public sealed class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.CurrentPassword).NotEmpty();

        RuleFor(x => x.NewPassword).NotEmpty();
    }
}
