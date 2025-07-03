using Conta360.Domain.Entities;
using Conta360.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Conta360.Infrastructure.Postgres.Contexts
{
    public class PostgresDbContext : DbContext, IApplicationDbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options) { }

        public DbSet<PgcAccount> PgcAccounts { get; set; } = null!;
        public DbSet<Transact> Transactions { get; set; } = null!;
        public DbSet<Account> Accounts { get; set; } = null!;

        // Implementación explícita de la interfaz para exponer solo IQueryable
        IQueryable<PgcAccount> IApplicationDbContext.PgcAccounts => PgcAccounts;
        IQueryable<Transact> IApplicationDbContext.Transactions => Transactions;
        IQueryable<Account> IApplicationDbContext.Accounts => Accounts;

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Puedes agregar lógica de auditoría aquí si lo deseas
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Account>().HasKey(a => a.Id);
            modelBuilder.Entity<Transact>().HasKey(t => t.Id);
            // Configuración adicional (índices, relaciones, etc.)
        }
    }
}