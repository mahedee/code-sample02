using MTShop.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTShop.Core.Entities.Admin
{
    public class TenantEntity : BaseEntity<Guid>
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
