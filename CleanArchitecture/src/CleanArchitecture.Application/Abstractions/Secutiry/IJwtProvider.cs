using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Abstractions.Secutiry;

public interface IJwtProvider
{
    (string Token, DateTime Expires) GenerateAccessToken(User user);
    string GenerateRefreshToken();
}