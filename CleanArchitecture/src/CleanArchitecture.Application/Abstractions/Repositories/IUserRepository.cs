using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string username, CancellationToken cancellationToken);
    void Insert(User user);
    void Update(User user);
}