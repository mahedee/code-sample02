namespace Domain.Entities.Common
{
    /// <summary>
    /// Base class for BaseAggregateRoot class
    /// Shared for all entities
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class BaseEntity<TKey> : IBaseEntity<TKey>
    {
        protected BaseEntity() { }
        protected BaseEntity(TKey id) => Id = id;

        //Implementation of interface
        public TKey Id { get; protected set; }
    }
}
