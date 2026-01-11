using Microsoft.Extensions.Logging;

namespace Payment.API.Services
{
    public interface IPaymentProcessor
    {
        Task<PaymentResult> ProcessPaymentAsync(decimal amount, string customerEmail);
    }

    public class PaymentProcessor : IPaymentProcessor
    {
        private readonly ILogger<PaymentProcessor> _logger;
        private readonly Random _random = new Random();

        public PaymentProcessor(ILogger<PaymentProcessor> logger)
        {
            _logger = logger;
        }

        public async Task<PaymentResult> ProcessPaymentAsync(decimal amount, string customerEmail)
        {
            _logger.LogInformation($"Processing payment of ${amount} for {customerEmail}");
            
            // Simulate payment processing time
            await Task.Delay(TimeSpan.FromSeconds(2));

            // Simulate payment success/failure (90% success rate for demo)
            var isSuccess = _random.Next(1, 101) <= 90;
            
            var result = new PaymentResult
            {
                IsSuccess = isSuccess,
                TransactionId = Guid.NewGuid().ToString("N")[..16].ToUpper(),
                Amount = amount,
                ProcessedAt = DateTime.UtcNow
            };

            if (!isSuccess)
            {
                var failureReasons = new[]
                {
                    "Insufficient funds",
                    "Card expired",
                    "Card declined",
                    "Invalid card number",
                    "Processing error"
                };
                result.FailureReason = failureReasons[_random.Next(failureReasons.Length)];
            }

            _logger.LogInformation($"Payment result: {(isSuccess ? "Success" : "Failed")} - {result.TransactionId}");
            return result;
        }
    }

    public class PaymentResult
    {
        public bool IsSuccess { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string FailureReason { get; set; }
    }
}