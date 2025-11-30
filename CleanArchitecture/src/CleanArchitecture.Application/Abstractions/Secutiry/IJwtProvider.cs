using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Abstractions.Secutiry;

public interface IJwtProvider
{
    (string AccessToken, DateTime ExpiresUtc) GenerateAccessToken(User user);
    (string RefreshToken, DateTime ExpiresUtc) GenerateRefreshToken();
}