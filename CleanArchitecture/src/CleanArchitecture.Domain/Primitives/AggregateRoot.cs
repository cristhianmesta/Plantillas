namespace CleanArchitecture.Domain.Primitives
{
    public abstract class AggregateRoot : Entity
    {
        private readonly List<IDomainEvent> _domainEvents = [];

        protected AggregateRoot(Guid id) : base(id)
        {
        }

        protected AggregateRoot()
        {
        }

        public IReadOnlyCollection<IDomainEvent> GetDomainEvents() 
            => [.. _domainEvents];

        protected void RaiseDomainEvent(IDomainEvent domainEvent) 
            => _domainEvents.Add(domainEvent);

        public void ClearDomainEvents()
    => _domainEvents.Clear();

    }
}
