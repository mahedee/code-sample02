using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string ProductName { get; set; } = string.Empty;
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal UnitPrice { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal TotalPrice { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string CustomerEmail { get; set; } = string.Empty;
        
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Shipped,
        Delivered
    }
}