using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Order.API.Data;
using Plain.RabbitMQ;
using Shared.Models;

namespace Order.API.Services
{
    public class PaymentResponseListener : IHostedService
    {
        private readonly ISubscriber _paymentSubscriber;
        private readonly IPublisher _publisher;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<PaymentResponseListener> _logger;

        public PaymentResponseListener(
            ISubscriber paymentSubscriber,
            IPublisher publisher,
            IServiceScopeFactory scopeFactory,
            ILogger<PaymentResponseListener> logger)
        {
            _paymentSubscriber = paymentSubscriber;
            _publisher = publisher;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _paymentSubscriber.Subscribe(ProcessPaymentResponse);
            _logger.LogInformation("Payment Response Listener started");
            return Task.CompletedTask;
        }

        private bool ProcessPaymentResponse(string message, IDictionary<string, object> headers)
        {
            try
            {
                _logger.LogInformation($"Received payment response: {message}");
                var response = JsonSerializer.Deserialize<PaymentProcessedEvent>(message);

                if (response == null)
                {
                    _logger.LogError("Failed to deserialize payment response");
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
                    // Payment successful - confirm order
                    order.Status = "Confirmed";
                    order.UpdatedAt = DateTime.UtcNow;
                    _logger.LogInformation($"Order {order.Id} confirmed after successful payment: {response.TransactionId}");
                }
                else
                {
                    // Payment failed - compensate by cancelling order and releasing inventory
                    order.Status = "Cancelled";
                    order.UpdatedAt = DateTime.UtcNow;
                    
                    _logger.LogWarning($"Order {order.Id} cancelled due to payment failure: {response.Message}");

                    // Publish inventory release event for compensation
                    var releaseEvent = new InventoryReleaseEvent
                    {
                        OrderId = response.OrderId,
                        ProductId = response.ProductId,
                        Quantity = order.Quantity,
                        Reason = "Payment failed"
                    };

                    _publisher.Publish(
                        JsonSerializer.Serialize(releaseEvent),
                        "inventory.release",
                        null);

                    _logger.LogInformation($"Published inventory release event for order {response.OrderId}");
                }

                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment response");
                return false;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Payment Response Listener stopped");
            return Task.CompletedTask;
        }
    }
}
