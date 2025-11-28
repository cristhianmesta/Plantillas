namespace CleanArchitecture.Domain.Primitives;

public interface IDomainEvent
{
    public Guid Id { get; init; }
}