using Microsoft.EntityFrameworkCore;
using MTShop.Application.Interfaces.Repositories;
using MTShop.Core.Entities;
using MTShop.Infrastructure.Persistence;

namespace MTShop.Infrastructure.Implementations.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MultitenantDbContext _context;

        public ProductRepository(MultitenantDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            //var product = new Product(name, description, rate);
            await _context.Products.AddAsync(product);
            return product;
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync() => await _context.Products.ToListAsync();

        public async Task<Product> GetByIdAsync(Guid id) => await _context.Products.FindAsync(id);
    }
}
