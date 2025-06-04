namespace Conta360.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync();
        void Rollback();
    }
}