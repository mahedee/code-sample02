using MTShop.Application.Common.Models;
using MTShop.Core.Entities.Identity;

namespace MTShop.Application
{
    public interface IMultitenantDbContextInitializer
    {
        Task SeedAsync(List<ApplicationRole>? defaultRoles, List<Users> defaultUsers, string tenantId);
    }
}
