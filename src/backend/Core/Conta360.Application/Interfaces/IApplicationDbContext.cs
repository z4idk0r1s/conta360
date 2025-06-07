using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Collections.Generic;

namespace Conta360.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<PgcAccount> PgcAccounts { get; set; }
        DbSet<Transaction> Transactions { get; set; }
        DbSet<Account> Accounts { get; set; }

        // Add other DbSet properties
        Task<int> SaveChangesAsync(CancellationToken cancellationToken  = default);        
    }
}