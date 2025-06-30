using Conta360.Domain.Entities;

namespace Conta360.Domain.Interfaces
{
    public interface IPgcAccountRepository : IRepository<PgcAccount>
    {
        Task<List<PgcAccount>> GetTreeStructureAsync();
        Task<List<PgcAccount>> GetByParentCodeAsync(string? parentCode);
        Task<List<PgcAccount>> GetRootAccountsAsync();
    }
}