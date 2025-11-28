using CleanArchitecture.Application.Abstractions.Repositories;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(string username, CancellationToken cancellationToken)
    {
        return await _context.Users.AnyAsync(u => u.Username == username, cancellationToken);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Insert(User user)
    {
        _context.Add(user);
    }

    public void Update(User user)
    {
        throw new NotImplementedException();
    }
}
