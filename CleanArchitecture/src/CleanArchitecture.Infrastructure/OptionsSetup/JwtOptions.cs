namespace CleanArchitecture.Infrastructure.OptionsSetup;

public record JwtOptions
{
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public string AccessTokenDurationInMinutes { get; init; } = string.Empty;
    public string RefreshTokenDurationInDays { get; init; } = string.Empty;
}
