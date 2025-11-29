using FluentValidation;

namespace CleanArchitecture.Application.UseCases.Users.Commands.ChangePassword;

public sealed class ChangePasswordValidator : AbstractValidator<ChangePassword>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.Username).NotNull().NotEmpty();
        RuleFor(x => x.OldPassword).NotNull().NotEmpty();
        RuleFor(x => x.NewPassword).NotNull().NotEmpty().MinimumLength(8);
    }
}