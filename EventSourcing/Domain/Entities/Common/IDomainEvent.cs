using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Common
{
    
    /// <summary>
    /// Interface for Domain Event
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IDomainEvent<out TKey>
    {
        public long AggregateVersion { get; }
        TKey AggregateId { get; }
        DateTime TimeStamp { get; }
    }
}
