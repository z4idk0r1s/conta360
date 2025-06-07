using Microsoft.EntityFrameworkCore;
using Conta360.Domain.Entities;
using Conta360.Infrastructure.Persistence.Contexts;

namespace Conta360.Infrastructure.Sqlite.Contexts
{
    public class SqliteDbContext : DbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options) { }

        DbSet<PgcAccount> PgcAccounts { get; set; }
        DbSet<Transaction> Transactions { get; set; }
        DbSet<Account> Accounts { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}