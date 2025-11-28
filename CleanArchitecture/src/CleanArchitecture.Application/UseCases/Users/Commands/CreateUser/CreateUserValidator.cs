using FluentValidation;

namespace CleanArchitecture.Application.UseCases.Users.Commands.CreateUser;

public sealed class CreateUserValidator : AbstractValidator<CreateUser>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Username).NotNull().NotEmpty();
        RuleFor(x => x.Email).NotNull().NotEmpty();
        RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(8);
    }
}
