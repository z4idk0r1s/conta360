using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Conta360.Domain.Entities;

namespace Conta360.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<PgcAccount> PgcAccounts { get; set; }
        DbSet<Transact> Transactions { get; set; }
        DbSet<Account> Accounts { get; set; }

        // Add other DbSet properties
        Task<int> SaveChangesAsync(CancellationToken cancellationToken  = default);        
    }
}