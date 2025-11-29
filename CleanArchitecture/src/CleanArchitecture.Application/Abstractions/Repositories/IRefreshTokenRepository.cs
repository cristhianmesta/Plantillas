using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Abstractions.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByToken(string token, CancellationToken cancellationToken);
    void CreateAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
    void UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
}
