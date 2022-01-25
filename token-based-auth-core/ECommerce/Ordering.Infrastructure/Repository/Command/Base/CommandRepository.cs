using Microsoft.EntityFrameworkCore;
using Ordering.Core.Repositories.Command.Base;
using Ordering.Infrastructure.Data;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repository.Command.Base
{
    // Generic command repository class
    public class CommandRepository<T> : ICommandRepository<T> where T : class
    {
        protected readonly OrderingContext _context;

        public CommandRepository(OrderingContext context)
        {
            _context = context;
        }

        // Insert
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // Update
        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Delete
        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
