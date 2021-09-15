using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(OrderingContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Customer>> GetCustomerByEmail(string email)
        {
            return await _context.Customers
                .Where(o => o.Email == email)
                .ToListAsync();
        }
    }
}
