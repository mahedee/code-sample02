using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MTShop.Application;
using MTShop.Application.Common.Models;
using MTShop.Application.Interfaces;
using MTShop.Core.Contracts;
using MTShop.Core.Entities.Admin;
using MTShop.Core.Entities.Base;

namespace MTShop.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly MultitenantDbContext _multitenantDbContext;
        private UserConfiguration _userConfiguration;
        private readonly IMultitenantDbContextInitializer _multitenantDbContextInitializer;
        private readonly ITenantService _tenantService;


        public string TenantId { get; set; }

        //Constructor
        public UnitOfWork(ICurrentUserService currentUserService, MultitenantDbContext multitenantDbContext,
            IOptions<UserConfiguration> userConfiguration, IMultitenantDbContextInitializer multitenantDbContextInitializer, 
            ITenantService tenantService)
        {
            _currentUserService = currentUserService;
            _multitenantDbContext = multitenantDbContext;
            _userConfiguration = userConfiguration.Value ?? throw new NullReferenceException(nameof(userConfiguration));
            _multitenantDbContextInitializer = multitenantDbContextInitializer ?? throw new NullReferenceException(nameof(multitenantDbContextInitializer));
            TenantId = _currentUserService?.TenantId ?? _tenantService.GetTenant()?.TId;
            _tenantService = tenantService;
        }
        public async Task<int> CommitAsync()
        {
            using (var transaction = await _multitenantDbContext.Database.BeginTransactionAsync())
            {

                try
                {
                    foreach (var entry in _multitenantDbContext.ChangeTracker.Entries<IMustHaveTenant>().ToList())
                    {
                        switch (entry.State)
                        {
                            case EntityState.Added:
                            case EntityState.Modified:
                                entry.Entity.TenantId = TenantId;
                                break;
                        }
                    }

                    foreach (var entry in _multitenantDbContext.ChangeTracker.Entries<BaseEntity<Guid>>())
                    {
                        switch (entry.State)
                        {
                            case EntityState.Added:
                                entry.Entity.CreatedBy = _currentUserService.UserId;
                                break;
                            case EntityState.Modified:
                                entry.Entity.ModifiedBy = _currentUserService.UserId;
                                break;

                            case EntityState.Deleted:
                                break;
                        }
                    }

                    var affectedRows = await _multitenantDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return affectedRows;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
                finally
                {
                    await _multitenantDbContext.Database.CloseConnectionAsync();
                    await _multitenantDbContext.DisposeAsync();
                }

            }
        }

        public void Dispose()
        {
            if (_multitenantDbContext != null)
            {
                _multitenantDbContext.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Database migration will execute here after a teanat is created.
        /// </summary>
        /// <param name="tenant"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task MigrateTenantDatabase(TenantEntity tenant)
        {
            var key = _currentUserService.TenantId;
            _multitenantDbContext.Database.SetConnectionString(tenant.ConnectionString);
            _multitenantDbContext.Database.Migrate();


            // Test it using debugger
            var defaultUsers = _userConfiguration.Users;
            var defaultRoles = _userConfiguration.Roles;

            await _multitenantDbContextInitializer.SeedAsync(defaultRoles, defaultUsers, tenant.TenantKey);
            await Task.CompletedTask;
        }
    }
}
