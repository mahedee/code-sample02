using System.Collections.Immutable;
using System.Reflection;

namespace Domain.Entities.Common
{
    public abstract class BaseAggregateRoot<TA, TKey> : BaseEntity<TKey>, IAggregateRoot<TKey> where TA : IAggregateRoot<TKey>
    {

        // Queuing all events
        private readonly Queue<IDomainEvent<TKey>> _events = new Queue<IDomainEvent<TKey>>();

        protected BaseAggregateRoot() { }
        protected BaseAggregateRoot(TKey id) : base(id)
        {

        }

        protected void AddEvent(IDomainEvent<TKey> @event)
        {
            if(@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            _events.Enqueue(@event);
            Apply(@event);
            Version++;
        }

        /// <summary>
        /// This method is implemented in the derived class
        /// Apply this method to implement different events
        /// </summary>
        /// <param name="event"></param>
        
        protected abstract void Apply(IDomainEvent<TKey> @event);

        // Implementation of IAggregateRoot
        // Aggregate version
        public long Version{ get; private set; }

        // Implementation of IAggregateRoot
        public IReadOnlyCollection<IDomainEvent<TKey>> Events => _events.ToImmutableArray();

        // Implementation of IAggregateRoot
        public void ClearEvents()
        {
            _events.Clear();
        }

        #region Factory

        private static readonly ConstructorInfo CTor;

        static BaseAggregateRoot()
        {
            var aggregateType = typeof(TA);
            CTor = aggregateType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null, new Type[0], new ParameterModifier[0]);
            if (null == CTor)
                throw new InvalidOperationException($"Unable to find required private parameterless constructor for Aggregate of type '{aggregateType.Name}'");
        }


        /// <summary>
        /// Create Base Aggregate root when Rehydrate all Events
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TA Create(IEnumerable<IDomainEvent<TKey>> events)
        {
            if (null == events || !events.Any())
                throw new ArgumentNullException(nameof(events));
            var result = (TA)CTor.Invoke(new object[0]);

            // Problem is in here
            var baseAggregate = result as BaseAggregateRoot<TA, TKey>;

            if (baseAggregate != null)
                foreach (var @event in events)
                    baseAggregate.AddEvent(@event);

            result.ClearEvents();

            return result;
        }

        #endregion  
    }
}
