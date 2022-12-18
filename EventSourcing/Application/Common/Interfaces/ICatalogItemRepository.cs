using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface ICatalogItemRepository
    {
        Task AddAsync(CatalogItem catalogItem);
        Task UpdateAsync(CatalogItem catalogItem);
    }
}
