using System.Threading.Tasks;

namespace Ordering.Core.Repositories.Command.Base
{
    // Generic interface for command repository
    public interface ICommandRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
