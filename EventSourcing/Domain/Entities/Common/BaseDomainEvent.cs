namespace Domain.Entities.Common
{
    /// <summary>
    /// Base domain event for all domain event
    /// </summary>
    /// <typeparam name="TA"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class BaseDomainEvent<TA, TKey> : IDomainEvent<TKey> where TA : IAggregateRoot<TKey>
    {
        protected BaseDomainEvent() { }
        public BaseDomainEvent(TA aggregateRoot)
        {
            if(aggregateRoot is null)
            {
                throw new ArgumentNullException(nameof(aggregateRoot));
            }

            AggregateId = aggregateRoot.Id;
            AggregateVersion = aggregateRoot.Version;
            TimeStamp = DateTime.Now;
        }
        //Implementation of IDomainEvent
        public long AggregateVersion { get; private set; }

        //Implementation of IDomainEvent
        public TKey AggregateId { get; private set; }

        //Implementation of IDomainEvent
        public DateTime TimeStamp { get; private set; }
    }
}
