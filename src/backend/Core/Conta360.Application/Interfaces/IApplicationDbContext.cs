using Conta360.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Conta360.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        IQueryable<PgcAccount> PgcAccounts { get; }
        IQueryable<Transact> Transactions { get; }
        IQueryable<Account> Accounts { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}