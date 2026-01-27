using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;
using EventBus;
using EventBus.Events;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderContext _context;
        private readonly IEventBus _eventBus;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(OrderContext context, IEventBus eventBus, ILogger<OrdersController> logger)
        {
            _context = context;
            _eventBus = eventBus;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            return order;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderRequest request)
        {
            var order = new Order
            {
                ProductId = request.ProductId,
                ProductName = request.ProductName,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice,
                TotalPrice = request.Quantity * request.UnitPrice,
                CustomerEmail = request.CustomerEmail
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Publish event
            var orderCreatedEvent = new OrderCreatedEvent
            {
                OrderId = order.Id,
                ProductId = order.ProductId,
                Quantity = order.Quantity,
                TotalPrice = order.TotalPrice,
                CustomerEmail = order.CustomerEmail
            };

            await _eventBus.PublishAsync(orderCreatedEvent);
            _logger.LogInformation($"Order created and event published: {order.Id}");

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, UpdateOrderStatusRequest request)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            order.Status = request.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("product-cache")]
        public async Task<ActionResult<IEnumerable<object>>> GetCachedProducts()
        {
            var cachedProducts = await _context.ProductCache
                .Select(p => new
                {
                    Id = p.ProductId,  // Return the actual ProductId from ProductService
                    p.Name,
                    p.Price,
                    p.Stock,
                    p.IsAvailable,
                    p.LastUpdated
                })
                .ToListAsync();

            return Ok(cachedProducts);
        }

        [HttpGet("pending-by-product/{productId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetPendingOrdersByProduct(int productId)
        {
            var orders = await _context.Orders
                .Where(o => o.ProductId == productId && o.Status == OrderStatus.Pending)
                .ToListAsync();

            return Ok(orders);
        }
    }

    public record CreateOrderRequest(
        int ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice,
        string CustomerEmail);

    public record UpdateOrderStatusRequest(OrderStatus Status);
}