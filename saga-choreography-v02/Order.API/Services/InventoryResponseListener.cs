using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Order.API.Data;
using Plain.RabbitMQ;
using Shared.Models;

namespace Order.API.Services
{
    public class InventoryResponseListener : IHostedService
    {
        private readonly ISubscriber _subscriber;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<InventoryResponseListener> _logger;

        public InventoryResponseListener(
            ISubscriber subscriber,
            IServiceScopeFactory scopeFactory,
            ILogger<InventoryResponseListener> logger)
        {
            _subscriber = subscriber;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriber.Subscribe(ProcessInventoryResponse);
            _logger.LogInformation("Inventory Response Listener started");
            return Task.CompletedTask;
        }

        private bool ProcessInventoryResponse(string message, IDictionary<string, object> headers)
        {
            try
            {
                _logger.LogInformation($"Received inventory response: {message}");
                var response = JsonSerializer.Deserialize<InventoryReservedEvent>(message);

                if (response == null)
                {
                    _logger.LogError("Failed to deserialize inventory response");
                    return false;
                }

                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

                var order = context.Orders.FirstOrDefault(o => o.Id == response.OrderId);
                if (order == null)
                {
                    _logger.LogWarning($"Order {response.OrderId} not found");
                    return true;
                }

                if (response.IsSuccess)
                {
                    // Move to next step - inventory reserved, waiting for payment
                    order.Status = "InventoryReserved";
                    order.UpdatedAt = DateTime.UtcNow;
                    _logger.LogInformation($"Order {order.Id} inventory reserved, awaiting payment processing");
                }
                else
                {
                    // Compensation: Cancel order due to inventory failure
                    order.Status = "Cancelled";
                    order.UpdatedAt = DateTime.UtcNow;
                    _logger.LogWarning($"Order {order.Id} cancelled due to inventory failure: {response.Message}");
                }

                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing inventory response");
                return false;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Inventory Response Listener stopped");
            return Task.CompletedTask;
        }
    }
}