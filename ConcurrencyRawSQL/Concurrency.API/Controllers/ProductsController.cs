using Concurrency.API.Models;
using Concurrency.API.Persistence;
using Concurrency.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Concurrency.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private DBConnector _connector;
        private readonly IProductService _productService;

        public ProductsController( DBConnector connector, IProductService productService)
        {
            _connector = connector;
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productService.GetProducts();

            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            var result = await _productService.UpdateProduct(product);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            var result = await _productService.AddProduct(product);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id, Guid rowVersion)
        {
            var result = await _productService.DeleteProduct(id, rowVersion);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
