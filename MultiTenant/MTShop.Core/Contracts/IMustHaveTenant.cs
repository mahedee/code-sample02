using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTShop.Core.Contracts
{
    public interface IMustHaveTenant
    {
        public string TenantId { get; set; }
    }
}
