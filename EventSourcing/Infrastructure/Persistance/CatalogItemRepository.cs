using Application.Common.Interfaces;
using Domain.Entities;

namespace Infrastructure.Persistance
{
    public class CatalogItemRepository : ICatalogItemRepository
    {
        private readonly ApplicationDbContext _context;

        public CatalogItemRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        public async Task AddAsync(CatalogItem catalogItem)
        {
            await _context.AddAsync(catalogItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CatalogItem catalogItem)
        {
            _context.Entry(catalogItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
