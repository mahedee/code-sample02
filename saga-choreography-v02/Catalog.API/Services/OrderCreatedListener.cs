using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Catalog.API.Data;
using Plain.RabbitMQ;
using Shared.Models;

namespace Catalog.API.Services
{
    public class OrderCreatedListener : IHostedService
    {
        private readonly ISubscriber _subscriber;
        private readonly IPublisher _publisher;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OrderCreatedListener> _logger;

        public OrderCreatedListener(
            ISubscriber subscriber,
            IPublisher publisher,
            IServiceScopeFactory scopeFactory,
            ILogger<OrderCreatedListener> logger)
        {
            _subscriber = subscriber;
            _publisher = publisher;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _subscriber.Subscribe(ProcessOrderCreated);
            _logger.LogInformation("Order Created Listener started");
            return Task.CompletedTask;
        }

        private bool ProcessOrderCreated(string message, IDictionary<string, object> headers)
        {
            InventoryReservedEvent? response = null;
            
            try
            {
                _logger.LogInformation($"Received order created event: {message}");
                var orderCreated = JsonSerializer.Deserialize<OrderCreatedEvent>(message);

                if (orderCreated == null)
                {
                    _logger.LogError("Failed to deserialize order created event");
                    return false;
                }

                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

                response = new InventoryReservedEvent
                {
                    OrderId = orderCreated.OrderId,
                    ProductId = orderCreated.ProductId,
                    TotalAmount = orderCreated.TotalAmount,
                    CustomerEmail = orderCreated.CustomerEmail
                };

                var product = context.Products.Find(orderCreated.ProductId);

                if (product == null)
                {
                    response.IsSuccess = false;
                    response.Message = $"Product with ID {orderCreated.ProductId} not found";
                    _logger.LogWarning($"Product {orderCreated.ProductId} not found for order {orderCreated.OrderId}");
                }
                else if (product.AvailableStock < orderCreated.Quantity)
                {
                    response.IsSuccess = false;
                    response.Message = $"Insufficient stock. Available: {product.AvailableStock}, Requested: {orderCreated.Quantity}";
                    _logger.LogWarning($"Insufficient stock for product {orderCreated.ProductId}. Available: {product.AvailableStock}, Requested: {orderCreated.Quantity}");
                }
                else
                {
                    // Reserve inventory (don't commit yet, wait for payment confirmation)
                    var originalStock = product.AvailableStock;
                    product.AvailableStock -= orderCreated.Quantity;
                    product.ReservedStock += orderCreated.Quantity;
                    product.UpdatedAt = DateTime.UtcNow;
                    
                    context.SaveChanges();

                    response.IsSuccess = true;
                    response.Message = "Inventory reserved successfully, awaiting payment";
                    response.ReservedQuantity = orderCreated.Quantity;
                    
                    _logger.LogInformation($"Inventory reserved for product {orderCreated.ProductId}. " +
                                         $"Available stock changed from {originalStock} to {product.AvailableStock}");
                }

                // Publish inventory response
                _publisher.Publish(
                    JsonSerializer.Serialize(response),
                    "inventory.reserved",
                    null);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing order created event");
                
                // Publish failure response if we have basic info
                if (response != null)
                {
                    response.IsSuccess = false;
                    response.Message = "Internal error processing inventory reservation";
                    
                    _publisher.Publish(
                        JsonSerializer.Serialize(response),
                        "inventory.reserved",
                        null);
                }

                return false;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Order Created Listener stopped");
            return Task.CompletedTask;
        }
    }
}