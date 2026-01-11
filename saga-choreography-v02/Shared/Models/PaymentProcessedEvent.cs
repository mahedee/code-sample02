namespace Shared.Models
{
    public class PaymentProcessedEvent
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public bool IsSuccess { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Message { get; set; }
        public string CustomerEmail { get; set; }
    }
}