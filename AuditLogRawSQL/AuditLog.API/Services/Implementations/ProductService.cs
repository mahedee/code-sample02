using AuditLog.API.Models;
using AuditLog.API.Repositories.Interfaces;
using AuditLog.API.Services.Interfaces;

namespace AuditLog.API.Services.Implementations
{
    public class ProductService : IProductService
    {
        IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public Task<Product> AddProduct(Product product)
        {
            return _productRepository.AddProduct(product);
        }

        public Task<Product> GetProduct(int id)
        {
            return _productRepository.GetProduct(id);
        }

        public Task<IEnumerable<Product>> GetProducts()
        {
            return _productRepository.GetProducts();
        }

        public Task<Product> UpdateProduct(Product product)
        {
            return _productRepository.UpdateProduct(product);
        }

        public Task<Product> DeleteProduct(int id)
        {
            return _productRepository.DeleteProduct(id);
        }
    }
}
