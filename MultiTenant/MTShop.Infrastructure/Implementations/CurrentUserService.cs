using Microsoft.AspNetCore.Http;
using MTShop.Application.Interfaces;
using System.Security.Claims;

namespace MTShop.Infrastructure.Implementations
{
    /// <summary>
    /// Process current user information from HttpContext
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private readonly HttpContext _httpContext;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }
        public string UserId => _httpContext?.User?.FindFirst("UserId")?.Value;

        public string UserName => _httpContext?.User?.FindFirst(x => x.Type == ClaimTypes.Name)?.Value;

        public string TenantId => _httpContext?.User?.FindFirst("TenantId")?.Value;
    }
}
