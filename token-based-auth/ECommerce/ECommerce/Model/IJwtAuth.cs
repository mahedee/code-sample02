namespace ECommerce.Model
{
    public interface IJwtAuth
    {
        string Authentication(string username, string password);
    }
}
