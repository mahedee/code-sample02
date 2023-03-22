
using MTShop.Core.Contracts;
using MTShop.Core.Entities.Base;

namespace MTShop.Core.Entities
{
    public class Product : BaseEntity<Guid>, IMustHaveTenant
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rate { get; set; }
        public string TenantId { get; set; }
    }
}
