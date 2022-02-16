namespace Ordering.Application.Common.Interfaces
{
    public interface ITokenGenerator
    {
        //public string GenerateToken(string userName, string password);
        public string GenerateJWTToken((string userId, string userName, IList<string> roles) userDetails);
    }
}
