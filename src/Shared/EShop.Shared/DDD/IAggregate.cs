namespace EShop.Shared.DDD;

public interface IAggregate : IEntity
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }

    IDomainEvent[] ClearDomainEvents();
}

public interface IAggregate<T> : IAggregate, IEntity<T>
{
     
}