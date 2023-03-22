using MTShop.Application.Common.Settings;

namespace MTShop.Application.Interfaces
{
    public interface ITenantService
    {
        public string GetDatabaseProvider();
        public string GetConnectionString();
        public Tenant GetTenant();
        //public TenantEntity GetTenant();
    }
}
