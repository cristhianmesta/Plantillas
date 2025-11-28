using CleanArchitecture.Application.Abstractions.Repositories;
using CleanArchitecture.Infrastructure.Persistence;

namespace CleanArchitecture.Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly AppDbContext _context = context;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) 
        => _context.SaveChangesAsync(cancellationToken);

}