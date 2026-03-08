using ECommerceApi.Models;

namespace ECommerceApi.Services;

public interface ITokenService
{
    string CreateToken(User user);
}
