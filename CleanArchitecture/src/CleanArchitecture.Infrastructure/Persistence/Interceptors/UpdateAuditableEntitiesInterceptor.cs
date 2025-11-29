using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using CleanArchitecture.Domain.Primitives;

namespace CleanArchitecture.Infrastructure.Persistence.Interceptors;
public sealed class UpdateAuditableEntitiesInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UpdateAuditableEntitiesInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
                            DbContextEventData eventData,
                            InterceptionResult<int> result,
                            CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        IEnumerable<EntityEntry<IAuditableEntity>> entries = dbContext.ChangeTracker.Entries<IAuditableEntity>();

        foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                var creator =  _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                entityEntry.Property(a => a.CreatedOnUtc).CurrentValue = DateTime.UtcNow;
                entityEntry.Property(a => a.CreatedBy).CurrentValue = creator ?? "system";

            }

            if (entityEntry.State == EntityState.Modified)
            {
                var editor = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                entityEntry.Property(a => a.ModifiedOnUtc).CurrentValue = DateTime.UtcNow;
                entityEntry.Property(a => a.ModifiedBy).CurrentValue = editor ?? "system";
                   
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
