namespace Conta360.Persistence.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync();
        void Rollback();
    }
}