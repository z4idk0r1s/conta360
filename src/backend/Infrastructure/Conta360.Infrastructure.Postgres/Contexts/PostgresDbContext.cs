using Microsoft.EntityFrameworkCore;
using Conta360.Domain.Entities;
using Conta360.Infrastructure.Persistence.Contexts;

namespace Conta360.Infrastructure.Postgres.Contexts
{
    public class PostgresDbContext : DbContext, IApplicationDbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Apply configurations for entities (e.g., Fluent API)
            modelBuilder.Entity<Account>().HasKey(a => a.Id);
            modelBuilder.Entity<Transaction>().HasKey(t => t.Id);
            // Add more configurations
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Add Audit Trail or other logic here
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}