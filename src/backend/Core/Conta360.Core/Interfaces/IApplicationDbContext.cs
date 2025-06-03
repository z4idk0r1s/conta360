using Microsoft.EntityFrameworkCore;
using Conta360.Domain.Entities;

namespace Conta360.Persistence.Contexts
{
    public interface IApplicationDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<Transaction> Transactions { get; set; }

        // Add other DbSet properties
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}