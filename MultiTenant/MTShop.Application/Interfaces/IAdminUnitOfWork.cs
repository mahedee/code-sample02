namespace MTShop.Application.Interfaces
{
    public interface IAdminUnitOfWork
    {
        Task<int> CommitAsync();
    }
}