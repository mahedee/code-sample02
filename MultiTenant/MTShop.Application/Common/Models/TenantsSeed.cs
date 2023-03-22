namespace MTShop.Application.Common.Models
{
    public class TenantsSeed
    {
        public bool IsDeleted { get; set; } = false;
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public string TenantName { get; set; }
        public string TenantKey { get; set; }
        public string DatabaseServer { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string ConnectionString { get; set; }
        public string DBProvider { get; set; }
    }
}
