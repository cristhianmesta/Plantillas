namespace CleanArchitecture.Application.Abstractions.Secutiry;

public sealed record AuthenticationTokenPair
{
    public string AccessToken { get; init; } = string.Empty;
    public DateTime AccessTokenExpiresOn { get; init; }
    public string RefreshToken { get; init; } = string.Empty;
    public DateTime RefreshTokenExpiresOn { get; init; }
}
