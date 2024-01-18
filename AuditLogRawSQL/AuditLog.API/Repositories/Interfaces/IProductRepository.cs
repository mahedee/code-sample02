using AuditLog.API.Models;

namespace AuditLog.API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(int id);
        Task<Product> AddProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task<Product> DeleteProduct(int id);
    }
}
