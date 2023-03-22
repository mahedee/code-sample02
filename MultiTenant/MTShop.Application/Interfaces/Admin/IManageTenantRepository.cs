using MTShop.Core.Entities.Admin;

namespace MTShop.Application.Interfaces.Admin
{
    public interface IManageTenantRepository
    {
        Task<IEnumerable<TenantEntity>> GetAllAsync();
        Task<TenantEntity> GetById(Guid id);
        Task<TenantEntity> GetByName(string name);
        //Task<TenantEntity> GetByTenantKey(string tenantKey);
        TenantEntity GetByTenantKey(string tenantKey);
        Task<TenantEntity> CreateAsync(TenantEntity tenant);
        Task<TenantEntity> UpdateAsync(TenantEntity updatedTenant);
        Task DeleteAsync(TenantEntity tenant);
        Task<string> GetConnectionString(string tenantKey);
        Task<bool> IsTenantExist(string tenantName);

    }
}
