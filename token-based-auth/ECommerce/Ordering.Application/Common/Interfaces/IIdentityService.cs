using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        // User section
        Task<(bool isSucceed, string userId)> CreateUserAsync(string userName, string password, string email, List<string> roles);
        Task<bool> SigninUserAsync(string userName, string password);
        Task<string> GetUserIdAsync(string userName);
        Task<(string userId, string UserName, IList<string> roles)> GetUserDetailsAsync(string userId);

        // Role Section
        Task<bool> CreateRoleAsync(string roleName);
    }
}
