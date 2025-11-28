using CleanArchitecture.Domain.Events;
using CleanArchitecture.Domain.Primitives;

namespace CleanArchitecture.Domain.Entities;

public class User : AggregateRoot //, IAuditableEntity
{
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    public string PasswordSalt { get; private set; }
    public string Email { get; private set; }
    public bool IsActive { get; private set; }
     public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    //public string CreatedBy { get; set; } = string.Empty;
    //public string? ModifiedBy { get; set; }
    //public DateTime CreatedOnUtc { get; set; } = DateTime.Now;
    //public DateTime? ModifiedOnUtc { get; set; }
    private User() { }

    private User(Guid id, string username, string email,  string passwrodHash, string passwordSalt) : base(id)
    {
        Username = username;
        Email = email;
        PasswordHash = passwrodHash;
        PasswordSalt = passwordSalt;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public static User Create(string username, string email, string passwrodHash, string passwordSalt)
    {
        var userId = Guid.NewGuid();
        var newUser = new User(userId, username, email, passwrodHash, passwordSalt);

        newUser.RaiseDomainEvent(
            new UserCreatedDomainEvent(Guid.NewGuid(), userId)
            );
        return newUser;
    }

    public void ChangePassword(string passwordHash, string passwordSalt)
    { 
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;    
    }

    public void Activate(bool isActive)
    { 
        IsActive = isActive;
    }
}