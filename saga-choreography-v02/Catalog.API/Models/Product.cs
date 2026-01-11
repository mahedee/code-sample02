using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.API.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public required string Name { get; set; }
        
        [MaxLength(1000)]
        public required string Description { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        
        public int AvailableStock { get; set; }
        public int ReservedStock { get; set; }
        public int MaxStockThreshold { get; set; } = 100;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // Computed property for total stock
        [NotMapped]
        public int TotalStock => AvailableStock + ReservedStock;
    }
}