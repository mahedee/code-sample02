using Microsoft.EntityFrameworkCore;
using MTShop.Application.Common.Exceptions;
using MTShop.Application.Interfaces.Admin;
using MTShop.Core.Entities.Admin;
using MTShop.Infrastructure.Persistence.Admin;

namespace MTShop.Infrastructure.Implementations.Admin
{
    public class ManageTenantRepository : IManageTenantRepository, IDisposable
    {
        private readonly AdminDbContext _adminDbContext;

        public ManageTenantRepository(AdminDbContext adminDbContext)
        {
            this._adminDbContext = adminDbContext;
        }
        public async Task<TenantEntity> CreateAsync(TenantEntity tenant)
        {
            await _adminDbContext.Tenants.AddAsync(tenant);
            return tenant;
        }

        public async Task DeleteAsync(TenantEntity tenant)
        {
            _adminDbContext.Remove(tenant);
            await Task.CompletedTask;
        }



        public async Task<IEnumerable<TenantEntity>> GetAllAsync()
        {
            return await _adminDbContext.Tenants.ToListAsync();
        }

        public async Task<TenantEntity> GetById(Guid id)
        {
            return await _adminDbContext.Tenants.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new NotFoundException("No tenant found with the given id");
        }

        public Task<TenantEntity> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public TenantEntity GetByTenantKey(string tenantKey)
        {
            //throw new NotImplementedException();
            var tenant = _adminDbContext.Tenants.First(x => x.TenantKey == tenantKey);
            return tenant;
        }

        public Task<string> GetConnectionString(string tenantKey)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsTenantExist(string tenantName)
        {
            return await _adminDbContext.Tenants.AnyAsync(x => x.TenantName.ToLower().Equals(tenantName.ToLower()));
        }

        public async Task<TenantEntity> UpdateAsync(TenantEntity updatedTenant)
        {
            //_adminDbContext.Tenants.AsNoTracking();

            _adminDbContext.Tenants.Update(updatedTenant);
            await Task.CompletedTask;
            return await GetById(updatedTenant.Id);
        }

        public void Dispose()
        {
            if (_adminDbContext != null)
                _adminDbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
