using ConcurrencyHandling.API.Data;
using ConcurrencyHandling.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConcurrencyHandling.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    // Concurrency check using ETags
    public class ProductsController : ControllerBase
    {
        private const string ETAG_HEADER = "ETag";
        private const string IF_MATCH_HEADER = "If-Match";

        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
          if (_context.Product == null)
          {
              return NotFound();
          }
            return await _context.Product.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          if (_context.Product == null)
          {
              return NotFound();
          }
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            // Add ETag header
            Response.Headers.Add(ETAG_HEADER, product.RecordVersion.ToString());
            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            var existingProduct = await _context.Product.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            // Check ETag header
            var existingProductETag = existingProduct.RecordVersion.ToString();
            var requestProductETag = Request.Headers[IF_MATCH_HEADER].FirstOrDefault();

            if (existingProductETag != requestProductETag)
            {
                // StatusCodes.Status412PreconditionFailed means "Precondition Failed"
                // meaning the ETag header value does not match the current ETag value
                return StatusCode(StatusCodes.Status412PreconditionFailed);
            }

            // Update the existing product with the new values
            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            existingProduct.RecordVersion = Guid.NewGuid();
            _context.Entry(existingProduct).State = EntityState.Modified;

            try
            {

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
          if (_context.Product == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Product'  is null.");
          }
            product.RecordVersion = Guid.NewGuid();
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            //if (_context.Product == null)
            //{
            //    return NotFound();
            //}
            var product = await _context.Product.FindAsync(id);

            if(product == null)
            {
                return NotFound();
            }

            var existingProductETag = product.RecordVersion.ToString();
            var requestProductETag = Request.Headers[IF_MATCH_HEADER].FirstOrDefault();

            if (existingProductETag != requestProductETag)
            {
                // StatusCodes.Status412PreconditionFailed means "Precondition Failed"
                // meaning the ETag header value does not match the current ETag value
                return StatusCode(StatusCodes.Status412PreconditionFailed);
            }
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //private bool ProductExists(int id)
        //{
        //    return (_context.Product?.Any(e => e.ProductId == id)).GetValueOrDefault();
        //}
    }
}
