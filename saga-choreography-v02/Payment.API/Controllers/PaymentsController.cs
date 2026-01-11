using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payment.API.Data;
using Payment.API.Models;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentDbContext _context;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(PaymentDbContext context, ILogger<PaymentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentTransaction>>> GetPayments()
        {
            return await _context.PaymentTransactions
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentTransaction>> GetPayment(int id)
        {
            var payment = await _context.PaymentTransactions.FindAsync(id);
            return payment == null ? NotFound() : payment;
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<PaymentTransaction>> GetPaymentByOrder(int orderId)
        {
            var payment = await _context.PaymentTransactions
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
            return payment == null ? NotFound() : payment;
        }

        [HttpGet("transaction/{transactionId}")]
        public async Task<ActionResult<PaymentTransaction>> GetPaymentByTransaction(string transactionId)
        {
            var payment = await _context.PaymentTransactions
                .FirstOrDefaultAsync(p => p.TransactionId == transactionId);
            return payment == null ? NotFound() : payment;
        }
    }
}