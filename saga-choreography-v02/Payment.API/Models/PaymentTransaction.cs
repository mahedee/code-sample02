using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.API.Models
{
    public class PaymentTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string CustomerEmail { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded
        
        [MaxLength(100)]
        public string TransactionId { get; set; }
        
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = "CreditCard";
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }
        
        [MaxLength(500)]
        public string FailureReason { get; set; }
        
        [MaxLength(100)]
        public string ExternalTransactionId { get; set; }
    }
}
