using System.ComponentModel.DataAnnotations;

namespace ECommerceApi.DTOs.Orders;

public class UpdateOrderStatusDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
}
