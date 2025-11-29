using CleanArchitecture.Application.Abstractions.Cqrs;
using CleanArchitecture.Application.Abstractions.Repositories;
using CleanArchitecture.Application.Abstractions.Secutiry;
using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Application.UseCases.Users.Commands.ChangePassword;

public sealed class ChangePasswordHandler : ICommandHandler<ChangePassword>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordHandler(IUnitOfWork unitOfWork, 
                                 IUserRepository userRepository,
                                 IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(ChangePassword command, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByUsernameAsync(command.Username, cancellationToken);
        if (user is null)
        {
            return Result.Failure(Error.Validation("Useranme", "El nombre de usuario no es válido"));
        }

        var isOldPasswordValid = _passwordHasher.VerifyPassword(command.OldPassword, user.PasswordHash, user.PasswordSalt);
        if (!isOldPasswordValid)
        {
            return Result.Failure(Error.Validation("Useranme", "Password antigua incorrecta"));
        }

        var (newHash, newSalt) = _passwordHasher.HashPassword(command.NewPassword);

        user.ChangePassword(newHash, newSalt);

        _userRepository.Update(user);

        var saved = await _unitOfWork.SaveChangesAsync();
        if (saved<=0)
        {
            Result.Failure(Error.Failure("ChangePassword", "Sucedio un error inesperado al cambiar la contraseña."));
        }

        return Result.Success();
    }
}
