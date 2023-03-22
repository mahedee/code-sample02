using MTShop.Core.Entities.Admin;

namespace MTShop.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
        Task MigrateTenantDatabase(TenantEntity tenant);
    }
}