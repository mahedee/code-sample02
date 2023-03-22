using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTShop.Core.Entities.Base
{
    public abstract class BaseEntity<TKey> where TKey : struct
    {
        public TKey Id { get; private set; }
        public bool IsDeleted { get; protected set; } = false;
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; protected set; } = DateTime.Now;
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; protected set; } = DateTime.Now;
    }
}
