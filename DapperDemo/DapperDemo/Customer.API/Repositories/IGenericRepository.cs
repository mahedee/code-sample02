using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.API.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(Int64 id);
        Task<int> CreateAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(T entity);
    }
}
