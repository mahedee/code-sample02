using EventBus.Events;
using ProductService.Data;
using Microsoft.EntityFrameworkCore;
using EventBus;

namespace ProductService.Services
{
    public class OrderEventHandler
    {
        private readonly ILogger<OrderEventHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public OrderEventHandler(ILogger<OrderEventHandler> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task Handle(OrderCreatedEvent @event)
        {
            _logger.LogInformation($"Order created event received: Order {@event.OrderId}, Product {@event.ProductId}, Quantity {@event.Quantity}");
            
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProductContext>();
            var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
            
            try
            {
                // Find the product
                var product = await context.Products.FindAsync(@event.ProductId);
                if (product != null)
                {
                    // Update product stock (reduce by order quantity)
                    if (product.Stock >= @event.Quantity)
                    {
                        var originalStock = product.Stock;
                        product.Stock -= @event.Quantity;
                        await context.SaveChangesAsync();
                        
                        _logger.LogInformation($"Product {product.Id} stock updated. Stock: {originalStock} → {product.Stock}");
                        
                        // Publish ProductUpdatedEvent to notify other services about stock change
                        var productUpdatedEvent = new ProductUpdatedEvent
                        {
                            ProductId = product.Id,
                            Name = product.Name,
                            Description = product.Description,
                            Price = product.Price,
                            Stock = product.Stock,
                            PreviousStock = originalStock,
                            PreviousPrice = product.Price  // Price didn't change
                        };

                        await eventBus.PublishAsync(productUpdatedEvent);
                        _logger.LogInformation($"ProductUpdatedEvent published for Product {product.Id} due to order {@event.OrderId}");
                        
                        // Check if stock is low
                        if (product.Stock <= 10)
                        {
                            _logger.LogWarning($"LOW STOCK ALERT: Product {product.Name} (ID: {product.Id}) has only {product.Stock} units remaining!");
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"Insufficient stock for Product {product.Id}. Available: {product.Stock}, Requested: {@event.Quantity}");
                    }
                }
                else
                {
                    _logger.LogWarning($"Product with ID {@event.ProductId} not found when processing order {@event.OrderId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing order created event for Order {@event.OrderId}");
            }
        }
    }
}