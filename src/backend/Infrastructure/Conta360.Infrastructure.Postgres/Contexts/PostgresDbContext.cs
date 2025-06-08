using Microsoft.EntityFrameworkCore;
using Conta360.Domain.Entities;
using Conta360.Application.Interfaces;

namespace Conta360.Infrastructure.Postgres.Contexts
{
    public class PostgresDbContext : DbContext, IApplicationDbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Transact> Transactions { get; set; } = null!;
        public DbSet<PgcAccount> PgcAccounts { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Apply configurations for entities (e.g., Fluent API)
            modelBuilder.Entity<Account>().HasKey(a => a.Id);
            modelBuilder.Entity<Transact>().HasKey(t => t.Id);
            // Add more configurations
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Add Audit Trail or other logic here
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}