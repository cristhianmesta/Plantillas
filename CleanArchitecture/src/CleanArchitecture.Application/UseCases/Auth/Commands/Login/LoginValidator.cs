using FluentValidation;

namespace CleanArchitecture.Application.UseCases.Auth.Commands.Login;

public sealed class LoginValidator : AbstractValidator<Login>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username).NotNull().NotEmpty();
        RuleFor(x => x.Password).NotNull().NotEmpty();
    }
}
