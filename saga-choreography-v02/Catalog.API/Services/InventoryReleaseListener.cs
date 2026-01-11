using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Catalog.API.Data;
using Plain.RabbitMQ;
using Shared.Models;

namespace Catalog.API.Services
{
    public class InventoryReleaseListener : IHostedService
    {
        private readonly ISubscriber _subscriber;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<InventoryReleaseListener> _logger;

        public InventoryReleaseListener(
            ISubscriber subscriber,
            IServiceScopeFactory scopeFactory,
            ILogger<InventoryReleaseListener> logger)
        {
            _subscriber = subscriber;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriber.Subscribe(ProcessInventoryRelease);
            _logger.LogInformation("Inventory Release Listener started");
            return Task.CompletedTask;
        }

        private bool ProcessInventoryRelease(string message, IDictionary<string, object> headers)
        {
            try
            {
                _logger.LogInformation($"Received inventory release event: {message}");
                var releaseEvent = JsonSerializer.Deserialize<InventoryReleaseEvent>(message);

                if (releaseEvent == null)
                {
                    _logger.LogError("Failed to deserialize inventory release event");
                    return false;
                }

                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

                var product = context.Products.Find(releaseEvent.ProductId);
                if (product == null)
                {
                    _logger.LogWarning($"Product {releaseEvent.ProductId} not found for inventory release");
                    return true; // Consider it processed
                }

                // Release reserved inventory back to available stock
                var beforeAvailable = product.AvailableStock;
                var beforeReserved = product.ReservedStock;
                
                product.AvailableStock += releaseEvent.Quantity;
                product.ReservedStock = Math.Max(0, product.ReservedStock - releaseEvent.Quantity);
                product.UpdatedAt = DateTime.UtcNow;
                
                context.SaveChanges();

                _logger.LogInformation($"Released inventory for product {releaseEvent.ProductId} due to: {releaseEvent.Reason}. " +
                                     $"Available: {beforeAvailable} → {product.AvailableStock}, " +
                                     $"Reserved: {beforeReserved} → {product.ReservedStock}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing inventory release event");
                return false;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Inventory Release Listener stopped");
            return Task.CompletedTask;
        }
    }
}