using Confluent.Kafka;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace EventBus
{
    public class KafkaEventBus : IEventBus, IDisposable
    {
        private readonly IProducer<string, string> _producer;
        private readonly IConsumer<string, string> _consumer;
        private readonly ILogger<KafkaEventBus> _logger;
        private readonly string _topicPrefix;
        private readonly Dictionary<string, Func<string, Task>> _handlers = new();
        private readonly List<string> _subscribedTopics = new();
        private bool _isConsuming = false;

        public KafkaEventBus(IConfiguration configuration, ILogger<KafkaEventBus> logger)
        {
            _logger = logger;
            _topicPrefix = configuration["Kafka:TopicPrefix"] ?? "microservices";

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                GroupId = configuration["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _producer = new ProducerBuilder<string, string>(producerConfig).Build();
            _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        }

        public async Task PublishAsync<T>(T @event) where T : class
        {
            var topic = $"{_topicPrefix}.{typeof(T).Name}";
            var message = JsonSerializer.Serialize(@event);

            try
            {
                var result = await _producer.ProduceAsync(topic, new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = message
                });

                _logger.LogInformation($"Event published to topic {topic}: {result.Status}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to publish event to topic {topic}");
                throw;
            }
        }

        public async Task SubscribeAsync<T>(Func<T, Task> handler) where T : class
        {
            var topic = $"{_topicPrefix}.{typeof(T).Name}";
            _logger.LogInformation($"Subscribing to topic: {topic}");
            
            try
            {
                // Store the handler for this topic
                _handlers[topic] = async (message) =>
                {
                    var @event = JsonSerializer.Deserialize<T>(message);
                    await handler(@event);
                };

                _subscribedTopics.Add(topic);
                _logger.LogInformation($"Added topic {topic} to subscription list");

                // Start the consumer if not already started
                if (!_isConsuming)
                {
                    await StartConsumerAsync();
                }
                else
                {
                    // Re-subscribe to all topics
                    _consumer.Subscribe(_subscribedTopics);
                    _logger.LogInformation($"Re-subscribed to all topics: {string.Join(", ", _subscribedTopics)}");
                }

                _logger.LogInformation($"Successfully subscribed to topic: {topic}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to subscribe to topic: {topic}");
                throw;
            }
        }

        private async Task StartConsumerAsync()
        {
            _isConsuming = true;
            _consumer.Subscribe(_subscribedTopics);
            _logger.LogInformation($"Subscribed to topics: {string.Join(", ", _subscribedTopics)}");

            _ = Task.Run(async () =>
            {
                _logger.LogInformation($"Starting consumer task for topics: {string.Join(", ", _subscribedTopics)}");
                while (true)
                {
                    try
                    {
                        _logger.LogDebug($"Polling for messages on topics: {string.Join(", ", _subscribedTopics)}...");
                        var consumeResult = _consumer.Consume(TimeSpan.FromMilliseconds(1000));
                        if (consumeResult != null)
                        {
                            var topic = consumeResult.Topic;
                            _logger.LogInformation($"Received message from topic {topic}: {consumeResult.Message.Value}");
                            
                            if (_handlers.TryGetValue(topic, out var handler))
                            {
                                await handler(consumeResult.Message.Value);
                                _consumer.Commit(consumeResult);
                                _logger.LogInformation($"Successfully processed message from topic {topic}");
                            }
                            else
                            {
                                _logger.LogWarning($"No handler found for topic {topic}");
                            }
                        }
                        else
                        {
                            _logger.LogDebug($"No message received from any topic, continuing polling...");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error consuming message from topics");
                    }
                }
            });
        }

        public void Dispose()
        {
            _producer?.Dispose();
            _consumer?.Dispose();
        }
    }
}