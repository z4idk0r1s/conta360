using Microsoft.EntityFrameworkCore;
using Conta360.Domain.Entities; // Asumiendo que tus entidades están en este namespace

namespace Conta360.Core.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Account> Accounts { get; set; }
        DbSet<Transaction> Transactions { get; set; }

        // Add other DbSet properties
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}