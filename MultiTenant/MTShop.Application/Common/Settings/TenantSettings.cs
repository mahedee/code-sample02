namespace MTShop.Application.Common.Settings
{
    public class TenantSettings
    {
        public DBConfiguration Defaults { get; set; }
        public List<Tenant> Tenants { get; set; }
    }
}
