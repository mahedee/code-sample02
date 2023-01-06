using Application.Common.Interfaces;
using Application.Common.Resolvers;
using Domain.Entities.Common;
using EventStore.Client;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Infrastructure.Persistance
{
    public class AggregateRepository<TA, TKey> : IAggregateRepository<TA, TKey> where TA : class, IAggregateRoot<TKey>
    {
        private readonly EventStoreClient _eventStoreClient;
        private readonly string _stramBaseName;

        public AggregateRepository(EventStoreClient eventStoreClient)
        {
            _eventStoreClient = eventStoreClient;
            var aggregateType = typeof(TA);
            _stramBaseName = aggregateType.Name;
        }
        public async Task AppendAsync(TA aggregate, CancellationToken cancellationToken = default)
        {
            if (null == aggregate)
                throw new ArgumentNullException(nameof(aggregate));
            if (!aggregate.Events.Any())
                return;

            var streamName = GetStreamName(aggregate.Id);

            var eventList = aggregate.Events.Select(Map).ToArray();

            var result = await _eventStoreClient.AppendToStreamAsync(streamName, StreamState.Any,
                eventList.ToArray(), cancellationToken: cancellationToken);
        }

        public async Task<Dictionary<int, object>> ReadEventsAsync(TKey aggregateId, CancellationToken cancellationToken = default)
        {
            var streamName = GetStreamName(aggregateId);
            var result = _eventStoreClient.ReadStreamAsync(Direction.Forwards, streamName, StreamPosition.Start);

            var events = new Dictionary<int, object>();
            var index = 0;

            foreach (var data in await result.ToListAsync(cancellationToken: cancellationToken))
            {
                // Read event metadata to get type information of an event
                var eventMetaData = JsonSerializer.Deserialize<EventMeta>(Encoding.UTF8.GetString(data.Event.Metadata.ToArray()));
                Type? typeInfo = Type.GetType(eventMetaData.EventType);
                if(typeInfo is null)
                {
                    throw new Exception($"Invalid type {eventMetaData.EventType}");
                }

                var jsonData = Encoding.UTF8.GetString(data.Event.Data.ToArray());
                var eventInfo = JsonConvert.DeserializeObject(jsonData, typeInfo, new JsonSerializerSettings()
                {
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                    ContractResolver = new PrivateSetterContractResolver()
                });

                events.Add(index, new
                {
                    Events = eventInfo,
                    EventType = data.OriginalEvent.EventType
                });

                index++;

            }

            return events;
        }


        /// <summary>
        /// Read all events using Aggregate ID
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<TA?> RehydreateAsync(TKey key, CancellationToken cancellationToken = default)
        {
            try
            {

                var streamName = GetStreamName(key);

                var events = new List<IDomainEvent<TKey>>();

                var result = _eventStoreClient.ReadStreamAsync(Direction.Forwards, streamName, StreamPosition.Start);


                foreach (var data in await result.ToListAsync(cancellationToken))
                {
                    //Read Event Metadata to get Type Information of an event
                    var eventMetaData = JsonSerializer.Deserialize<EventMeta>
                        (Encoding.UTF8.GetString(data.Event.Metadata.ToArray()));

                    Type? typeInfo = Type.GetType(eventMetaData.EventType);

                    if (typeInfo == null)
                    {
                        throw new Exception($"Invalid type {eventMetaData.EventType}");
                    }

                    var jsonData = Encoding.UTF8.GetString(data.Event.Data.ToArray());
                    var eventInfo = JsonConvert.DeserializeObject(jsonData, typeInfo, new JsonSerializerSettings()
                    {
                        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                        ContractResolver = new PrivateSetterContractResolver()
                    });


                    events.Add((IDomainEvent<TKey>)eventInfo);
                }

                if (!events.Any())
                    return null;

                var aggregateResult = BaseAggregateRoot<TA, TKey>.Create(events.OrderBy(x => x.AggregateVersion));

                return aggregateResult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Techical: Expression-bodied member
        // Generate stream name format
        private string GetStreamName(TKey aggregateKey)
            => $"{_stramBaseName}_{aggregateKey}";

        // Map domain event to event data
        private static EventData Map(IDomainEvent<TKey> @event)
        {
            var meta = new EventMeta()
            {
                EventType = @event.GetType().AssemblyQualifiedName
            };

            var metaJson = System.Text.Json.JsonSerializer.Serialize(meta);
            var metadata = Encoding.UTF8.GetBytes(metaJson);

            var eventData = new EventData(
                Uuid.NewUuid(),
                @event.GetType().Name,
                JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                }),
                metadata
                );
            return eventData;
        }

        /// <summary>
        ///  Meta data information for an event which will also saved into each Event Payload
        /// </summary>
        internal struct EventMeta
        {
            public string EventType { get; set; }
        }
    }
}
