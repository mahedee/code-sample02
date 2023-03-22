namespace MTShop.Application.DTOs
{
    public class TenantDTO
    {
        public string TenantName { get; set; }
        public string TenantKey { get; set; }
        public string DatabaseServer { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string ConnectionString { get; set; }
        public string DBProvider { get; set; }
    }
}
