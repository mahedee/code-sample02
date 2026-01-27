using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class ProductCache
    {
        public int Id { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public int Stock { get; set; }
        
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        
        public bool IsAvailable { get; set; } = true;
    }
}