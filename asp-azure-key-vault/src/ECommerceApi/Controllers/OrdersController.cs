using ECommerceApi.Data;
using ECommerceApi.DTOs.Orders;
using ECommerceApi.Models;
using ECommerceApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IEmailService _emailService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(AppDbContext db, IEmailService emailService, ILogger<OrdersController> logger)
    {
        _db = db;
        _emailService = emailService;
        _logger = logger;
    }

    /// <summary>Place a new order. Stocks are decremented automatically.</summary>
    [HttpPost]
    public async Task<ActionResult<OrderResponseDto>> CreateOrder(CreateOrderDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _db.Users.FindAsync(userId);
        if (user is null) return Unauthorized();

        var productIds = dto.Items.Select(i => i.ProductId).Distinct().ToList();
        var products = await _db.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        var order = new Order { UserId = userId };

        foreach (var item in dto.Items)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
                return BadRequest(new { message = $"Product ID {item.ProductId} not found." });

            if (product.Stock < item.Quantity)
                return BadRequest(new { message = $"Insufficient stock for '{product.Name}'. Available: {product.Stock}." });

            product.Stock -= item.Quantity;
            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
            order.TotalAmount += item.Quantity * product.Price;
        }

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        // Fire-and-forget order confirmation email
        _ = _emailService.SendOrderConfirmationAsync(user.Email, user.Name, order.Id, order.TotalAmount)
            .ContinueWith(t => _logger.LogWarning(t.Exception, "Order confirmation email failed for order #{OrderId}", order.Id),
                TaskContinuationOptions.OnlyOnFaulted);

        return CreatedAtAction(nameof(GetById), new { id = order.Id }, MapToDto(order, products));
    }

    /// <summary>Get the current user's orders.</summary>
    [HttpGet]
    public async Task<ActionResult<List<OrderResponseDto>>> GetMyOrders()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var orders = await _db.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return Ok(orders.Select(o => MapToDto(o)));
    }

    /// <summary>Get a specific order by ID. Admins can view any order.</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponseDto>> GetById(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isAdmin = User.IsInRole("Admin");

        var order = await _db.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order is null) return NotFound();
        if (!isAdmin && order.UserId != userId) return Forbid();

        return Ok(MapToDto(order));
    }

    /// <summary>Update order status. E.g. Pending → Shipped → Delivered.</summary>
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateOrderStatusDto dto)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order is null) return NotFound();

        order.Status = dto.Status;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static OrderResponseDto MapToDto(Order order, Dictionary<int, Product>? productMap = null)
    {
        return new OrderResponseDto
        {
            Id = order.Id,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            CreatedAt = order.CreatedAt,
            Items = order.Items.Select(i => new OrderItemResponseDto
            {
                ProductId = i.ProductId,
                ProductName = productMap != null
                    ? (productMap.TryGetValue(i.ProductId, out var p) ? p.Name : string.Empty)
                    : i.Product?.Name ?? string.Empty,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
    }
}
