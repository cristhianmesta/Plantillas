namespace CleanArchitecture.Domain.Entities;
public class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string Token { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? ReplacedByToken { get; set; }


    private RefreshToken()
    {
        
    }

    private RefreshToken(Guid id, User user, string token, DateTime expiresAt)
    {
        Id = id;
        User = user;
        Token = token;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.Now;
    }

    public static RefreshToken Create(User user, string token, DateTime expiresAt)
    { 
        var id = Guid.NewGuid();
        return new RefreshToken(id, user, token, expiresAt);
    }

    public void Revoked()
    { 
        RevokedAt = DateTime.Now; 
    }

    public void Rotate(string replacedByToken) 
    {
        Revoked();
        ReplacedByToken = replacedByToken;
    }

}