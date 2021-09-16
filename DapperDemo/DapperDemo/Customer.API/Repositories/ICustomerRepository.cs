using Ordering.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.API.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<List<Customer>> GetAllByEmailId(string email);
    }
}
