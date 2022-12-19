namespace CleanArch.Application.Common.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
