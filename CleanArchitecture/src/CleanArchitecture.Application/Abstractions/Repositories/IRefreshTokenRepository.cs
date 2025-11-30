using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Abstractions.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken);
    void Insert(RefreshToken refreshToken, CancellationToken cancellationToken);
    void Update(RefreshToken refreshToken, CancellationToken cancellationToken);
}
