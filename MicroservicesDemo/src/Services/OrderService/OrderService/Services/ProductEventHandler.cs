using EventBus.Events;
using OrderService.Data;
using OrderService.Models;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Services
{
    public class ProductEventHandler
    {
        private readonly ILogger<ProductEventHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public ProductEventHandler(ILogger<ProductEventHandler> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task Handle(ProductCreatedEvent @event)
        {
            _logger.LogInformation($"Product created event received: {@event.ProductId} - {@event.Name}");
            
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OrderContext>();
            
            try
            {
                // Cache the product information locally for faster order processing
                var cachedProduct = new ProductCache
                {
                    ProductId = @event.ProductId,
                    Name = @event.Name,
                    Description = "", // ProductCreatedEvent doesn't include description
                    Price = @event.Price,
                    Stock = @event.Stock,
                    IsAvailable = @event.Stock > 0,
                    LastUpdated = DateTime.UtcNow
                };

                context.ProductCache.Add(cachedProduct);
                await context.SaveChangesAsync();
                
                _logger.LogInformation($"Product {@event.ProductId} cached locally. Stock: {@event.Stock}, Available: {cachedProduct.IsAvailable}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error caching product created event for Product {@event.ProductId}");
            }
        }

        public async Task Handle(ProductUpdatedEvent @event)
        {
            _logger.LogInformation($"Product updated event received: {@event.ProductId} - {@event.Name}");
            
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OrderContext>();
            
            try
            {
                // Update cached product information
                var cachedProduct = await context.ProductCache
                    .FirstOrDefaultAsync(p => p.ProductId == @event.ProductId);
                if (cachedProduct != null)
                {
                    cachedProduct.Name = @event.Name;
                    cachedProduct.Description = @event.Description;
                    cachedProduct.Price = @event.Price;
                    cachedProduct.Stock = @event.Stock;
                    cachedProduct.IsAvailable = @event.Stock > 0;
                    cachedProduct.LastUpdated = DateTime.UtcNow;

                    await context.SaveChangesAsync();
                    
                    _logger.LogInformation($"Product {@event.ProductId} cache updated. Stock: {@event.Stock}, Available: {cachedProduct.IsAvailable}");

                    // Check for price changes and log important business events
                    if (@event.Price != @event.PreviousPrice)
                    {
                        _logger.LogWarning($"PRICE CHANGE: Product {@event.ProductId} price changed from ${@event.PreviousPrice:F2} to ${@event.Price:F2}");
                        
                        // Check pending orders with old price
                        await CheckPendingOrdersForPriceChanges(@event.ProductId, @event.Price, @event.PreviousPrice, context);
                    }

                    // Check for stock changes
                    if (@event.Stock != @event.PreviousStock)
                    {
                        if (@event.Stock == 0)
                        {
                            _logger.LogWarning($"OUT OF STOCK: Product {@event.ProductId} is now out of stock");
                            await CheckPendingOrdersForOutOfStock(@event.ProductId, context);
                        }
                        else if (@event.PreviousStock == 0 && @event.Stock > 0)
                        {
                            _logger.LogInformation($"BACK IN STOCK: Product {@event.ProductId} is now available with {@event.Stock} units");
                        }
                    }
                }
                else
                {
                    _logger.LogWarning($"Product {@event.ProductId} not found in cache when processing update event");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing product updated event for Product {@event.ProductId}");
            }
        }

        private async Task CheckPendingOrdersForPriceChanges(int productId, decimal newPrice, decimal oldPrice, OrderContext context)
        {
            // Find pending orders for this product
            var pendingOrders = await context.Orders
                .Where(o => o.ProductId == productId && o.Status == OrderStatus.Pending && o.UnitPrice == oldPrice)
                .ToListAsync();

            if (pendingOrders.Any())
            {
                _logger.LogWarning($"Found {pendingOrders.Count} pending orders for Product {productId} with old price ${oldPrice:F2}. New price: ${newPrice:F2}");
                
                foreach (var order in pendingOrders)
                {
                    _logger.LogInformation($"Order {order.Id} may need price adjustment: ordered at ${order.UnitPrice:F2}, current price ${newPrice:F2}");
                }
            }
        }

        private async Task CheckPendingOrdersForOutOfStock(int productId, OrderContext context)
        {
            // Find pending orders for out-of-stock products
            var pendingOrders = await context.Orders
                .Where(o => o.ProductId == productId && o.Status == OrderStatus.Pending)
                .ToListAsync();

            if (pendingOrders.Any())
            {
                _logger.LogWarning($"ALERT: Found {pendingOrders.Count} pending orders for out-of-stock Product {productId}");
                
                foreach (var order in pendingOrders)
                {
                    _logger.LogWarning($"Order {order.Id} for Product {productId} may need to be cancelled - product out of stock");
                }
            }
        }
    }
}
