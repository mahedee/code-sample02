using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Catalog.API.Data;
using Plain.RabbitMQ;
using Shared.Models;

namespace Catalog.API.Services
{
    public class PaymentConfirmationListener : IHostedService
    {
        private readonly ISubscriber _subscriber;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<PaymentConfirmationListener> _logger;

        public PaymentConfirmationListener(
            ISubscriber subscriber,
            IServiceScopeFactory scopeFactory,
            ILogger<PaymentConfirmationListener> logger)
        {
            _subscriber = subscriber;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriber.Subscribe(ProcessPaymentConfirmation);
            _logger.LogInformation("Payment Confirmation Listener started");
            return Task.CompletedTask;
        }

        private bool ProcessPaymentConfirmation(string message, IDictionary<string, object> headers)
        {
            try
            {
                _logger.LogInformation($"Received payment confirmation: {message}");
                var paymentEvent = JsonSerializer.Deserialize<PaymentProcessedEvent>(message);

                if (paymentEvent == null)
                {
                    _logger.LogError("Failed to deserialize payment event");
                    return false;
                }

                if (paymentEvent.IsSuccess)
                {
                    // Payment successful - inventory reservation is now committed
                    // No action needed, inventory was already reserved
                    _logger.LogInformation($"Payment confirmed for order {paymentEvent.OrderId}, inventory reservation committed");
                }
                // Note: If payment fails, the InventoryReleaseListener will handle releasing the inventory

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment confirmation");
                return false;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Payment Confirmation Listener stopped");
            return Task.CompletedTask;
        }
    }
}