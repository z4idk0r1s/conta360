using Microsoft.EntityFrameworkCore;
using Conta360.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Conta360.Persistence.Contexts
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