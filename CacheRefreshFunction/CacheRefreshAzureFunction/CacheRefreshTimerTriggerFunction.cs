using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CacheRefreshAzureFunction
{
    public class CacheRefreshTimerTriggerFunction
    {
        private readonly ILogger _logger;
        private static readonly HttpClient httpClient = new HttpClient();

        public CacheRefreshTimerTriggerFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CacheRefreshTimerTriggerFunction>();
        }

        [Function("CacheRefreshTimerTriggerFunction")]
        public async Task RunAsync([TimerTrigger("*/5 * * * * *")] ScheduleTimerInfo timerInfo)
        {
            _logger.LogInformation($"Cache refresh function executed at: {DateTime.UtcNow}");

            try
            {
                // Fetch fresh data from API (e.g., Exchange Rates API)
                string apiUrl = "https://api.exchangerate-api.com/v4/latest/USD";
                string responseData = await httpClient.GetStringAsync(apiUrl);

                // Connect to Redis Cache
                // ConnectionMultiplexer object is used to connect to the Redis server
                // IDatabase object is used to interact with the Redis database
                string cacheConnectionString = Environment.GetEnvironmentVariable("RedisConnectionString")?.Trim();
  

                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(cacheConnectionString);
                IDatabase cache = redis.GetDatabase();

                // Store data in Redis Cache (Cache expiration: 5 minutes)
                // Cache expiration means that the stored data will be automatically removed from the cache after 5 minutes.
                // This helps ensure that the cache does not hold stale data and is refreshed periodically.
                cache.StringSet("ExchangeRates", responseData, TimeSpan.FromMinutes(5));

                // Retrieve data from cache
                _logger.LogInformation("Exchange rates updated successfully in Redis Cache.");

                // Retrieve data from cache
                string cachedData = cache.StringGet("ExchangeRates");


                // Show the cached data in the log

                if (!string.IsNullOrEmpty(cachedData))
                {
                    // Parse the JSON response
                    var exchangeRates = JObject.Parse(cachedData);

                    // Extract specific rates
                    var usdToBdt = exchangeRates["rates"]["BDT"].Value<decimal>();
                    var usdToCad = exchangeRates["rates"]["CAD"].Value<decimal>();

                    _logger.LogInformation("Current exchange rate:");
                    _logger.LogInformation($"USD to BDT: {usdToBdt}");
                    _logger.LogInformation($"USD to CAD: {usdToCad}");
                }
                else
                {
                    _logger.LogWarning("Cache is empty! Data retrieval failed.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating and retrieving cache: {ex.Message}");
            }

            if (timerInfo.IsPastDue)
            {
                _logger.LogWarning("The function is running later than scheduled.");
            }

            timerInfo.Last = DateTime.Now;
            timerInfo.Next = timerInfo.Last.AddSeconds(5);
        }
    }

    public class ScheduleTimerInfo
    {
        public DateTime Last { get; set; } = DateTime.Now;
        public DateTime Next { get; set; } = DateTime.Now.AddSeconds(5);

        // This property is used to determine if the timer is past due
        // It defines a method that returns true if the current time is greater than the Next property
        public bool IsPastDue => DateTime.Now > Next;
    }
}
