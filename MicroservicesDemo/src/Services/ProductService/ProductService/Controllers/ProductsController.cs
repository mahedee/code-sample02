using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;
using EventBus;
using EventBus.Events;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _context;
        private readonly IEventBus _eventBus;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ProductContext context, IEventBus eventBus, ILogger<ProductsController> logger)
        {
            _context = context;
            _eventBus = eventBus;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Publish event
            var productCreatedEvent = new ProductCreatedEvent
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock
            };

            await _eventBus.PublishAsync(productCreatedEvent);
            _logger.LogInformation($"Product created and event published: {product.Name}");

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest();

            // Get the original product to track changes
            var originalProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (originalProduct == null)
                return NotFound();

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                
                // Publish product updated event
                var productUpdatedEvent = new ProductUpdatedEvent
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    PreviousPrice = originalProduct.Price,
                    PreviousStock = originalProduct.Stock
                };

                await _eventBus.PublishAsync(productUpdatedEvent);
                _logger.LogInformation($"Product updated and event published: {product.Name}. Price: ${originalProduct.Price} → ${product.Price}, Stock: {originalProduct.Stock} → {product.Stock}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<IEnumerable<object>>> GetLowStockProducts([FromQuery] int threshold = 10)
        {
            var lowStockProducts = await _context.Products
                .Where(p => p.Stock <= threshold)
                .Select(p => new 
                {
                    p.Id,
                    p.Name,
                    p.Stock,
                    p.Price,
                    Status = p.Stock == 0 ? "Out of Stock" : "Low Stock"
                })
                .ToListAsync();

            return Ok(lowStockProducts);
        }

        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] UpdateStockRequest request)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            var originalStock = product.Stock;
            product.Stock = request.NewStock;
            await _context.SaveChangesAsync();

            // Publish ProductUpdatedEvent for stock changes
            var productUpdatedEvent = new ProductUpdatedEvent
            {
                ProductId = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                PreviousStock = originalStock,
                PreviousPrice = product.Price  // Price didn't change, so previous = current
            };

            await _eventBus.PublishAsync(productUpdatedEvent);
            _logger.LogInformation($"Stock updated for product {product.Name}: {originalStock} → {request.NewStock} and event published");
            
            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }

    public record UpdateStockRequest(int NewStock);
}