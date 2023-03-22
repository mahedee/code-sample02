using Microsoft.EntityFrameworkCore;
using MTShop.Application.Interfaces;
using MTShop.Core.Entities.Base;
using MTShop.Infrastructure.Persistence.Admin;

namespace MTShop.Infrastructure.Persistence
{
    public class AdminUnitOfWork : IAdminUnitOfWork, IDisposable
    {
        private readonly AdminDbContext _context;

        public AdminUnitOfWork(AdminDbContext context)
        {
            _context = context ?? throw new NullReferenceException(nameof(context));
        }

        /// <summary>
        /// Execute save changes with additional information.
        /// </summary>
        /// <returns></returns>
        public async Task<int> CommitAsync()
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {

                try
                {
                    foreach (var entry in _context.ChangeTracker.Entries<BaseEntity<Guid>>())
                    {
                        switch (entry.State)
                        {
                            case EntityState.Added:
                            case EntityState.Modified:
                                entry.Entity.CreatedBy = "Admin";
                                break;
                            case EntityState.Deleted:
                                break;
                        }
                    }

                    var affectedRows = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return affectedRows;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
                finally
                {
                    await _context.Database.CloseConnectionAsync();
                    await _context.DisposeAsync();
                }
            }
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
