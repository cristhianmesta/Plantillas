
using CleanArchitecture.Application.Abstractions.Cqrs;
using CleanArchitecture.Application.Abstractions.Repositories;
using CleanArchitecture.Application.Abstractions.Secutiry;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Application.UseCases.Auth.Commands.Login;

public sealed class LoginHandler : ICommandHandler<Login, AuthenticationTokenPair>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginHandler(IUnitOfWork unitOfWork,
                        IUserRepository userRepository,
                        IRefreshTokenRepository refreshTokenRepository,
                        IPasswordHasher passwordHasher,
                        IJwtProvider jwtProvider)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<AuthenticationTokenPair>> Handle(Login command, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByUsernameAsync(command.Username, cancellationToken);

        if (user is null)
        {
            return Result.Failure<AuthenticationTokenPair>(Error.Validation("Login", "El nombre de usuario no es válido."));
        }

        var isPasswordValid = _passwordHasher.VerifyPassword(command.Password, user.PasswordHash, user.PasswordSalt);

        if (!isPasswordValid)
        {
            return Result.Failure<AuthenticationTokenPair>(Error.Validation("Login", "Credenciales no válidas"));
        }

        var ( accesToken, accessTokenExpiresOn) = _jwtProvider.GenerateAccessToken(user);
        var ( refreshToken, refreshTokenExpiresOn ) = _jwtProvider.GenerateRefreshToken();

        var newRefreshToken = RefreshToken.Create(user, refreshToken, refreshTokenExpiresOn);

        _refreshTokenRepository.Insert(newRefreshToken, cancellationToken);

        var saved = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (saved <= 0)
        {
            //TO-DO - Escribir en error en el LOG;
            
            return Result.Failure<AuthenticationTokenPair>(Error.Failure("Login", "Sucedió un error inesperado"));
        }

        return Result.Success(new AuthenticationTokenPair { 
            AccessToken = accesToken,
            AccessTokenExpiresOn = accessTokenExpiresOn,
            RefreshToken = refreshToken,
            RefreshTokenExpiresOn = refreshTokenExpiresOn
        });

    }
}