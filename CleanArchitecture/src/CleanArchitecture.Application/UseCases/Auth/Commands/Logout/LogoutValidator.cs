using FluentValidation;

namespace CleanArchitecture.Application.UseCases.Auth.Commands.Logout;

public sealed class LogoutValidator : AbstractValidator<Logout>
{
    public LogoutValidator()
    {
        RuleFor(x => x.RefreshToken).NotNull().NotEmpty();
    }
}
