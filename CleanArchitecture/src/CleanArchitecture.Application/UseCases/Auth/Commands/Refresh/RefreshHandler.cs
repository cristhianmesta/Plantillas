using CleanArchitecture.Application.Abstractions.Cqrs;
using CleanArchitecture.Application.Abstractions.Repositories;
using CleanArchitecture.Application.Abstractions.Secutiry;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Application.UseCases.Auth.Commands.Refresh;

public sealed class RefreshHandler : ICommandHandler<Refresh, AuthenticationTokenPair>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public RefreshHandler(IUnitOfWork unitOfWork,
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

    public async Task<Result<AuthenticationTokenPair>> Handle(Refresh command, CancellationToken cancellationToken = default)
    {
        var currentRefreshToken = await _refreshTokenRepository.GetByTokenAsync(command.RefreshToken, cancellationToken);
        if (currentRefreshToken == null)
        {
            return Result.Failure<AuthenticationTokenPair>(Error.Validation("refreshToken", "Refresh token no válido."));
        }

        var user = await _userRepository.GetByIdAsync(currentRefreshToken.UserId, cancellationToken);
        if (user == null)
        {
            return Result.Failure<AuthenticationTokenPair>(Error.Validation("refreshToken", "Refresh token no válido."));
        }

        var (accesToken, accessTokenExpiresOn) = _jwtProvider.GenerateAccessToken(user);
        var (refreshToken, refreshTokenExpiresOn) = _jwtProvider.GenerateRefreshToken();

        var newRefreshToken = RefreshToken.Create(user, refreshToken, refreshTokenExpiresOn);
        currentRefreshToken.Rotate(newRefreshToken.Token);

        _refreshTokenRepository.Update(currentRefreshToken, cancellationToken);
        _refreshTokenRepository.Insert(newRefreshToken, cancellationToken);

        var saved = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (saved <= 0)
        {
            //TO-DO - Escribir en error en el LOG;

            return Result.Failure<AuthenticationTokenPair>(Error.Failure("refreshToken", "Sucedió un error inesperado."));
        }

        return Result.Success(new AuthenticationTokenPair
        {
            AccessToken = accesToken,
            AccessTokenExpiresOn = accessTokenExpiresOn,
            RefreshToken = refreshToken,
            RefreshTokenExpiresOn = refreshTokenExpiresOn
        });

    }
}