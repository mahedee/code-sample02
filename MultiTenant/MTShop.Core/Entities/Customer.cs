using MTShop.Core.Contracts;
using MTShop.Core.Entities.Base;

namespace MTShop.Core.Entities
{
    public class Customer : BaseEntity<Guid>, IMustHaveTenant
    {
        public string FullName { get; set; }
        public string FathersName { get; set; }
        public string MothersName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string TenantId { get; set; }
    }
}
