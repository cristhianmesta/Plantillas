using CleanArchitecture.Domain.Primitives;

namespace CleanArchitecture.Domain.Events;

public record UserCreatedDomainEvent(Guid Id, Guid UserId) : IDomainEvent;
