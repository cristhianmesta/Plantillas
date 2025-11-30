using FluentValidation;

namespace CleanArchitecture.Application.UseCases.Auth.Commands.Refresh;

public sealed class RefreshValidator : AbstractValidator<Refresh>
{
    public RefreshValidator()
    {
        RuleFor(x => x.RefreshToken).NotNull().NotEmpty();
    }
}
