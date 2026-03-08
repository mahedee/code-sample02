using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.DTOs.Orders;

public class CreateOrderDto
{
    [Required, MinLength(1, ErrorMessage = "Order must contain at least one item.")]
    public List<OrderItemRequestDto> Items { get; set; } = new();
}

public class OrderItemRequestDto
{
    [Required]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
}
