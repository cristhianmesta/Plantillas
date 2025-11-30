
using CleanArchitecture.Application.Abstractions.Cqrs;
using CleanArchitecture.Application.Abstractions.Repositories;
using CleanArchitecture.Application.Abstractions.Secutiry;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Application.UseCases.Auth.Commands.Logout;

public sealed record Logout(string RefreshToken) : ICommand;


public sealed class LogoutHandler : ICommandHandler<Logout>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LogoutHandler(IUnitOfWork unitOfWork,
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

    public async Task<Result> Handle(Logout command, CancellationToken cancellationToken = default)
    {
        var currentRefreshToken = await _refreshTokenRepository.GetByTokenAsync(command.RefreshToken, cancellationToken);
        if (currentRefreshToken == null )
        {
            return Result.Success();
        }

        if (currentRefreshToken.RevokedAt != null)
        {
            return Result.Success();
        }

        currentRefreshToken.Revoked();
        _refreshTokenRepository.Update(currentRefreshToken, cancellationToken);

        var saved = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (saved <= 0)
        {
            //TO-DO - Escribir en error en el LOG;

            return Result.Failure<AuthenticationTokenPair>(Error.Failure("refreshToken", "Sucedió un error inesperado."));
        }

        return Result.Success();
    }
}