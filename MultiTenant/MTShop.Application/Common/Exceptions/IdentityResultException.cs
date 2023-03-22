using Microsoft.AspNetCore.Identity;

namespace MTShop.Application.Common.Exceptions
{
    public class IdentityResultException : Exception
    {
        public string ErrorMessage { get; set; }
        public IdentityResultException(IdentityResult identityResult, string? message) : base(message)
        {
            ErrorMessage = string.Join("\n", identityResult.Errors.Select(x => x.Description).ToList());
        }
    }
}
