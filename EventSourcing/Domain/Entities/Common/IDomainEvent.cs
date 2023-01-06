namespace Domain.Entities.Common
{

    /// <summary>
    /// Interface for Domain Event
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IDomainEvent<out TKey>
    {
        public long AggregateVersion { get; }
        TKey AggregateId { get; }
        DateTime TimeStamp { get; }
    }
}
