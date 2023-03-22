namespace MTShop.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string UserName { get; }
        string TenantId { get; }
    }
}
