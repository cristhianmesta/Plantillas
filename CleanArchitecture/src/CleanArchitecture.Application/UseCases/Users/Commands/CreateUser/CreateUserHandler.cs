using CleanArchitecture.Application.Abstractions.Cqrs;
using CleanArchitecture.Application.Abstractions.Repositories;
using CleanArchitecture.Application.Abstractions.Secutiry;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Application.UseCases.Users.Commands.CreateUser;

public sealed class CreateUserHandler : ICommandHandler<CreateUser, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserHandler(IUnitOfWork unitOfWork,  IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<Guid>> Handle(CreateUser command, CancellationToken cancellationToken = default)
    {

        if (await _userRepository.ExistsAsync(command.Username, cancellationToken))
        {
            return Result.Failure<Guid>(Error.Validation("Username", "El usuarme ingresado ya existe."));
        }
       
        var (hash, salt) = _passwordHasher.HashPassword(command.Password);
        var newUser = User.Create(command.Username, command.Email, hash, salt);

        _userRepository.Insert(newUser);
        var saved = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (saved <= 0)
        {
            return Result.Failure<Guid>(Error.Failure("Usuario","No se pudo guardar el usuario."));
        }

        return Result.Success(newUser.Id);
    }
}
