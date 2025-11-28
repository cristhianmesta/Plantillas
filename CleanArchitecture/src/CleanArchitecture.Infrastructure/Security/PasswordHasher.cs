using CleanArchitecture.Application.Abstractions.Secutiry;
using System.Security.Cryptography;

namespace CleanArchitecture.Infrastructure.Security;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;      // 128 bits
    private const int KeySize = 32;       // 256 bits
    private const int Iterations = 100000; // recomendado >= 100k
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

    public (string Hash, string Salt) HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            Algorithm,
            KeySize
        );

        return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
    }

    public bool VerifyPassword(string password, string hash, string salt)
    {
        byte[] hashByte = Convert.FromBase64String(hash);
        byte[] saltByte = Convert.FromBase64String(salt);

        byte[] hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
            hashByte,
            saltByte,
            Iterations,
            Algorithm,
            KeySize
        );

        return CryptographicOperations.FixedTimeEquals(hashToCompare, saltByte);
    }
}