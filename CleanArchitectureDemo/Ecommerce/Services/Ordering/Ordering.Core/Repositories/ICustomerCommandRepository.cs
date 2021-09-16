using Ordering.Core.Entities;
using Ordering.Core.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Core.Repositories
{
    public interface ICustomerCommandRepository : IRepository<Customer>
    {
        //Custom operation which is not generic
        Task<IEnumerable<Customer>> GetCustomerByEmail(string email);
    }
}
