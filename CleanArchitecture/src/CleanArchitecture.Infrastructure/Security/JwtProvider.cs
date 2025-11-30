using CleanArchitecture.Application.Abstractions.Secutiry;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.OptionsSetup;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CleanArchitecture.Infrastructure.Security;

public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;

    public JwtProvider(IOptions<JwtOptions> options) => _options = options.Value;

    public (string AccessToken, DateTime ExpiresUtc) GenerateAccessToken(User user)
    {
        var tokenClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // -- Aqui se pueden ingresar más Claims


        // --

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_options.SecretKey);
        var signingCredentials = new SigningCredentials(
                                        new SymmetricSecurityKey(key), 
                                        SecurityAlgorithms.HmacSha256
                                        );

        var tokenDurationInMinutes = int.Parse(_options.AccessTokenDurationInMinutes ?? "15");
        var tokenExpires = DateTime.UtcNow.AddMinutes(tokenDurationInMinutes);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(tokenClaims),
            Expires = tokenExpires,
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            SigningCredentials = signingCredentials
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return ( tokenHandler.WriteToken(token) , tokenExpires);
    }


    public (string RefreshToken, DateTime ExpiresUtc) GenerateRefreshToken()
    {
        var days = int.Parse(_options.RefreshTokenDurationInDays ?? "1");
        DateTime expires = DateTime.UtcNow.AddDays(days);
        string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        return (token, expires);
    }
}