using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MTShop.Application.Common.Constants;
using MTShop.Application.Common.Exceptions;
using MTShop.Application.Common.Settings;
using MTShop.Application.Interfaces;
using MTShop.Application.Interfaces.Admin;

namespace MTShop.Infrastructure.Implementations
{

    // Managing tenant by tenant key
    public class TenantService : ITenantService
    {
        private readonly IManageTenantRepository _manageTenantRepository;
        private readonly HttpContext _httpContext;
        private Tenant _currentTenant;
        private TenantSettings _tenantSettings;
        //private TenantEntity _currentTenant;

        public TenantService(IManageTenantRepository manageTenantRepository, 
            IHttpContextAccessor httpContextAccessor, IOptions<TenantSettings> tenantSettings)
        {
            _manageTenantRepository = manageTenantRepository;
            _httpContext = httpContextAccessor.HttpContext;
            _tenantSettings = tenantSettings.Value ?? throw new NullReferenceException(nameof(tenantSettings));

            if (_httpContext != null)
            {

                // gets tenant key from token
                var tenantID = _httpContext.User?.FindFirst("TenantId")?.Value;
                if(tenantID != null)
                {
                    SetTenant(tenantID);
                }
                else if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                {
                    SetTenant(tenantId);
                }
                else
                {
                    throw new Exception("Invalid Tenant");
                }
            }
        }

        // Verifying tenant information by TenantKey from database and config file
        private async void SetTenant(string tenantId)
        {
            if (tenantId == UserRolesConstants.SuperAdmin)
                return;

            var tenantFromDb = _manageTenantRepository.GetByTenantKey(tenantId);
            Tenant _tenantMapped = new Tenant();
            if (tenantFromDb != null)
            {
                _tenantMapped = new()
                {
                    Name = tenantFromDb.TenantName,
                    ConnectionString = tenantFromDb.ConnectionString,
                    TId = tenantFromDb.TenantKey
                };

                _currentTenant = _tenantMapped
                    ?? throw new NotFoundException("Invalid Tenant you are looking for.");

            }
            else
            {
                _currentTenant = _tenantSettings.Tenants.Where(x => x.TId == tenantId).FirstOrDefault()
                    ?? throw new NotFoundException("Invalid Tenant you are looking for.");
            }


        }

        public string GetConnectionString()
        {
            return _currentTenant?.ConnectionString;
        }

        public string GetDatabaseProvider()
        {
            //return _tenantSettings?.Defaults?.DBProvider;
            return _tenantSettings?.Defaults?.DBProvider;
        }

        public Tenant GetTenant()
        {
            return _currentTenant;
        }
    }
}
