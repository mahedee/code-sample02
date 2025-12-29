using EcommerceApi.Entities;
using EcommerceApi.Repositories;
using EcommerceApi.Metrics;
using Microsoft.AspNetCore.Mvc;
using App.Metrics;
using System.Text.Json;

namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMetrics _metrics;

        public ProductController(IProductRepository productRepository, IMetrics metrics)
        {
            _productRepository = productRepository;
            _metrics = metrics;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            // Start timing the operation
            using var timer = _metrics.Measure.Timer.Time(MetricsRegistry.GetAllProductsTimer);
            
            try
            {
                // Increment API call counter
                _metrics.Measure.Counter.Increment(MetricsRegistry.GetAllProductsCounter);

                var products = await _productRepository.GetAllAsync();
                
                // Update business metrics
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.TotalProductsGauge, products.Count());
                
                // Calculate and update average product price
                if (products.Any())
                {
                    var averagePrice = products.Average(p => p.Price);
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.AverageProductPriceGauge, (double)averagePrice);
                }

                // Track response size
                var responseJson = JsonSerializer.Serialize(products);
                var responseSize = System.Text.Encoding.UTF8.GetByteCount(responseJson);
                _metrics.Measure.Histogram.Update(MetricsRegistry.ResponseSizeHistogram, responseSize);

                // Update API health score (100 for successful operations)
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ApiHealthScoreGauge, 100);

                return Ok(products);
            }
            catch (Exception)
            {
                // Track server errors
                _metrics.Measure.Counter.Increment(MetricsRegistry.ServerErrorCounter);
                
                // Update API health score on error
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ApiHealthScoreGauge, 0);
                
                throw;
            }
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            // Start timing the operation
            using var timer = _metrics.Measure.Timer.Time(MetricsRegistry.GetProductByIdTimer);
            
            try
            {
                // Increment API call counter
                _metrics.Measure.Counter.Increment(MetricsRegistry.GetByProductIdCounter);
                
                // Track product views for business analytics
                _metrics.Measure.Counter.Increment(MetricsRegistry.ProductViewsCounter);

                var product = await _productRepository.GetByIdAsync(id);
                
                if (product == null)
                {
                    // Track not found errors
                    _metrics.Measure.Counter.Increment(MetricsRegistry.ProductNotFoundCounter);
                    
                    // Update API health score for client errors (75 - not as bad as server errors)
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.ApiHealthScoreGauge, 75);
                    
                    return NotFound();
                }

                // Track response size
                var responseJson = JsonSerializer.Serialize(product);
                var responseSize = System.Text.Encoding.UTF8.GetByteCount(responseJson);
                _metrics.Measure.Histogram.Update(MetricsRegistry.ResponseSizeHistogram, responseSize);

                // Update API health score for successful operations
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ApiHealthScoreGauge, 100);

                return Ok(product);
            }
            catch (Exception)
            {
                // Track server errors
                _metrics.Measure.Counter.Increment(MetricsRegistry.ServerErrorCounter);
                
                // Update API health score on server error
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ApiHealthScoreGauge, 0);
                
                throw;
            }
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            // Start timing the operation
            using var timer = _metrics.Measure.Timer.Time(MetricsRegistry.CreateProductTimer);
            
            try
            {
                // Increment API call counter
                _metrics.Measure.Counter.Increment(MetricsRegistry.CreateProductCounter);

                // Track request size
                if (product != null)
                {
                    var requestJson = JsonSerializer.Serialize(product);
                    var requestSize = System.Text.Encoding.UTF8.GetByteCount(requestJson);
                    _metrics.Measure.Histogram.Update(MetricsRegistry.RequestSizeHistogram, requestSize);
                }

                // Validation checks
                if (product == null || string.IsNullOrWhiteSpace(product.Name))
                {
                    // Track validation errors
                    _metrics.Measure.Counter.Increment(MetricsRegistry.ValidationErrorCounter);
                    
                    // Update API health score for validation errors
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.ApiHealthScoreGauge, 80);
                    
                    return BadRequest("Product name is required");
                }

                var createdProduct = await _productRepository.CreateAsync(product);
                
                // Track business metrics
                _metrics.Measure.Counter.Increment(MetricsRegistry.ProductCreatedTodayCounter);
                
                // Update total products count
                var allProducts = await _productRepository.GetAllAsync();
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.TotalProductsGauge, allProducts.Count());
                
                // Update average price
                if (allProducts.Any())
                {
                    var averagePrice = allProducts.Average(p => p.Price);
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.AverageProductPriceGauge, (double)averagePrice);
                }

                // Track response size
                var responseJson = JsonSerializer.Serialize(createdProduct);
                var responseSize = System.Text.Encoding.UTF8.GetByteCount(responseJson);
                _metrics.Measure.Histogram.Update(MetricsRegistry.ResponseSizeHistogram, responseSize);

                // Update API health score for successful operations
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ApiHealthScoreGauge, 100);

                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
            }
            catch (Exception)
            {
                // Track server errors
                _metrics.Measure.Counter.Increment(MetricsRegistry.ServerErrorCounter);
                
                // Update API health score on server error
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ApiHealthScoreGauge, 0);
                
                throw;
            }
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            // Start timing the operation
            using var timer = _metrics.Measure.Timer.Time(MetricsRegistry.UpdateProductTimer);
            
            try
            {
                // Increment API call counter
                _metrics.Measure.Counter.Increment(MetricsRegistry.UpdateProductCounter);

                // Track request size
                if (product != null)
                {
                    var requestJson = JsonSerializer.Serialize(product);
                    var requestSize = System.Text.Encoding.UTF8.GetByteCount(requestJson);
                    _metrics.Measure.Histogram.Update(MetricsRegistry.RequestSizeHistogram, requestSize);
                }

                // Validation checks
                if (product == null || id != product.Id)
                {
                    // Track validation errors
                    _metrics.Measure.Counter.Increment(MetricsRegistry.ValidationErrorCounter);
                    
                    // Update API health score for validation errors
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.ApiHealthScoreGauge, 80);
                    
                    return BadRequest("Product ID mismatch");
                }

                if (string.IsNullOrWhiteSpace(product.Name))
                {
                    // Track validation errors
                    _metrics.Measure.Counter.Increment(MetricsRegistry.ValidationErrorCounter);
                    
                    // Update API health score for validation errors
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.ApiHealthScoreGauge, 80);
                    
                    return BadRequest("Product name is required");
                }

                var existingProduct = await _productRepository.GetByIdAsync(id);
                if (existingProduct == null)
                {
                    // Track not found errors
                    _metrics.Measure.Counter.Increment(MetricsRegistry.ProductNotFoundCounter);
                    
                    // Update API health score for client errors
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.ApiHealthScoreGauge, 75);
                    
                    return NotFound();
                }

                var updatedProduct = await _productRepository.UpdateAsync(product);
                
                // Update business metrics after successful update
                var allProducts = await _productRepository.GetAllAsync();
                if (allProducts.Any())
                {
                    var averagePrice = allProducts.Average(p => p.Price);
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.AverageProductPriceGauge, (double)averagePrice);
                }

                // Update API health score for successful operations
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ApiHealthScoreGauge, 100);

                return Ok(updatedProduct);
            }
            catch (Exception)
            {
                // Track server errors
                _metrics.Measure.Counter.Increment(MetricsRegistry.ServerErrorCounter);
                
                // Update API health score on server error
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ApiHealthScoreGauge, 0);
                
                throw;
            }
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Start timing the operation
            using var timer = _metrics.Measure.Timer.Time(MetricsRegistry.DeleteProductTimer);
            
            try
            {
                // Increment API call counter
                _metrics.Measure.Counter.Increment(MetricsRegistry.DeleteProductCounter);

                var result = await _productRepository.DeleteAsync(id);
                
                if (!result)
                {
                    // Track not found errors
                    _metrics.Measure.Counter.Increment(MetricsRegistry.ProductNotFoundCounter);
                    
                    // Update API health score for client errors
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.ApiHealthScoreGauge, 75);
                    
                    return NotFound();
                }

                // Update business metrics after successful deletion
                var allProducts = await _productRepository.GetAllAsync();
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.TotalProductsGauge, allProducts.Count());
                
                // Update average price
                if (allProducts.Any())
                {
                    var averagePrice = allProducts.Average(p => p.Price);
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.AverageProductPriceGauge, (double)averagePrice);
                }
                else
                {
                    _metrics.Measure.Gauge.SetValue(MetricsRegistry.AverageProductPriceGauge, 0);
                }

                // Update API health score for successful operations
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ApiHealthScoreGauge, 100);

                return NoContent();
            }
            catch (Exception)
            {
                // Track server errors
                _metrics.Measure.Counter.Increment(MetricsRegistry.ServerErrorCounter);
                
                // Update API health score on server error
                _metrics.Measure.Gauge.SetValue(MetricsRegistry.ApiHealthScoreGauge, 0);
                
                throw;
            }
        }
    }
}