using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Order.API.Data;
using Plain.RabbitMQ;
using Shared.Models;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderDbContext _context;
        private readonly IPublisher _publisher;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(
            OrderDbContext context, 
            IPublisher publisher, 
            ILogger<OrdersController> logger)
        {
            _context = context;
            _publisher = publisher;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Order>>> GetOrders()
        {
            return await _context.Orders.OrderByDescending(o => o.CreatedAt).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            return order == null ? NotFound() : order;
        }

        [HttpPost]
        public async Task<ActionResult<Models.Order>> CreateOrder(CreateOrderRequest request)
        {
            try
            {
                var order = new Models.Order
                {
                    ProductId = request.ProductId,
                    ProductName = request.ProductName,
                    Quantity = request.Quantity,
                    UnitPrice = request.UnitPrice,
                    TotalAmount = request.Quantity * request.UnitPrice,
                    CustomerEmail = request.CustomerEmail
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Order {order.Id} created, publishing OrderCreated event");

                // Publish order created event
                var orderCreatedEvent = new OrderCreatedEvent
                {
                    OrderId = order.Id,
                    ProductId = order.ProductId,
                    ProductName = order.ProductName,
                    Quantity = order.Quantity,
                    UnitPrice = order.UnitPrice,
                    TotalAmount = order.TotalAmount,
                    CustomerEmail = order.CustomerEmail
                };

                _publisher.Publish(
                    JsonSerializer.Serialize(orderCreatedEvent),
                    "order.created",
                    null);

                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return StatusCode(500, "Internal server error");
            }
        }
    }

    public class CreateOrderRequest
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string CustomerEmail { get; set; }
    }
}