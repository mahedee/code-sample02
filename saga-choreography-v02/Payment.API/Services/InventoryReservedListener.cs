using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Payment.API.Data;
using Payment.API.Models;
using Payment.API.Services;
using Plain.RabbitMQ;
using Shared.Models;

namespace Payment.API.Services
{
    public class InventoryReservedListener : IHostedService
    {
        private readonly ISubscriber _subscriber;
        private readonly IPublisher _publisher;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<InventoryReservedListener> _logger;

        public InventoryReservedListener(
            ISubscriber subscriber,
            IPublisher publisher,
            IServiceScopeFactory scopeFactory,
            ILogger<InventoryReservedListener> logger)
        {
            _subscriber = subscriber;
            _publisher = publisher;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriber.Subscribe(ProcessInventoryReserved);
            _logger.LogInformation("Inventory Reserved Listener started");
            return Task.CompletedTask;
        }

        private bool ProcessInventoryReserved(string message, IDictionary<string, object> headers)
        {
            PaymentProcessedEvent response = null;
            
            try
            {
                _logger.LogInformation($"Received inventory reserved event: {message}");
                var inventoryEvent = JsonSerializer.Deserialize<InventoryReservedEvent>(message);

                if (inventoryEvent == null)
                {
                    _logger.LogError("Failed to deserialize inventory reserved event");
                    return false;
                }

                response = new PaymentProcessedEvent
                {
                    OrderId = inventoryEvent.OrderId,
                    ProductId = inventoryEvent.ProductId,
                    Amount = inventoryEvent.TotalAmount,
                    CustomerEmail = inventoryEvent.CustomerEmail
                };

                if (!inventoryEvent.IsSuccess)
                {
                    // Inventory reservation failed, no payment needed
                    response.IsSuccess = false;
                    response.Message = "Payment skipped due to inventory failure";
                    _logger.LogInformation($"Skipping payment for order {inventoryEvent.OrderId} due to inventory failure");
                }
                else
                {
                    // Process payment
                    using var scope = _scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();
                    var paymentProcessor = scope.ServiceProvider.GetRequiredService<IPaymentProcessor>();

                    // Create payment record
                    var payment = new PaymentTransaction
                    {
                        OrderId = inventoryEvent.OrderId,
                        ProductId = inventoryEvent.ProductId,
                        Amount = inventoryEvent.TotalAmount,
                        CustomerEmail = inventoryEvent.CustomerEmail,
                        Status = "Processing"
                    };

                    context.PaymentTransactions.Add(payment);
                    context.SaveChanges();

                    // Process payment
                    var paymentResult = paymentProcessor.ProcessPaymentAsync(
                        inventoryEvent.TotalAmount, 
                        inventoryEvent.CustomerEmail).Result;

                    // Update payment record
                    payment.Status = paymentResult.IsSuccess ? "Completed" : "Failed";
                    payment.TransactionId = paymentResult.TransactionId;
                    payment.ProcessedAt = paymentResult.ProcessedAt;
                    payment.FailureReason = paymentResult.FailureReason;
                    payment.ExternalTransactionId = paymentResult.TransactionId;

                    context.SaveChanges();

                    // Prepare response
                    response.IsSuccess = paymentResult.IsSuccess;
                    response.TransactionId = paymentResult.TransactionId;
                    response.Message = paymentResult.IsSuccess 
                        ? "Payment processed successfully" 
                        : $"Payment failed: {paymentResult.FailureReason}";

                    _logger.LogInformation($"Payment processing completed for order {inventoryEvent.OrderId}: " +
                                         $"{(paymentResult.IsSuccess ? "Success" : "Failed")}");
                }

                // Publish payment response
                _publisher.Publish(
                    JsonSerializer.Serialize(response),
                    "payment.processed",
                    null);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing inventory reserved event");
                
                // Publish failure response
                if (response != null)
                {
                    response.IsSuccess = false;
                    response.Message = "Internal error processing payment";
                    
                    _publisher.Publish(
                        JsonSerializer.Serialize(response),
                        "payment.processed",
                        null);
                }

                return false;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Inventory Reserved Listener stopped");
            return Task.CompletedTask;
        }
    }
}
