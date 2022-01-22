using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<(bool isSucceed, string userId)> CreateUserAsync(string userName, string password, string email, List<string> roles);
    }
}
