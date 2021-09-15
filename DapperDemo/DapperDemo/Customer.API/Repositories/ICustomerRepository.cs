using Ordering.API.Entities;
using Ordering.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
    }
}
