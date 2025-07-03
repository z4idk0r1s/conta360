using Conta360.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Conta360.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Conta360.Infrastructure.Sqlite.Contexts
{
    /// <summary>
    /// Implementación de IApplicationDbContext usando SQLite y respetando la exposición de DbSet.
    /// </summary>
    public class SqliteDbContext : DbContext, IApplicationDbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options) { }

        IQueryable<PgcAccount> IApplicationDbContext.PgcAccounts => PgcAccounts;
        IQueryable<Transact> IApplicationDbContext.Transactions => Transactions;
        IQueryable<Account> IApplicationDbContext.Accounts => Accounts;

        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Transact> Transactions { get; set; } = null!;
        public DbSet<PgcAccount> PgcAccounts { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Índice único para el código PGC
            modelBuilder.Entity<PgcAccount>()
                .HasIndex(a => a.Code)
                .IsUnique();

            // Relación padre‐hijo en PgcAccount
            modelBuilder.Entity<PgcAccount>()
                .HasMany(a => a.Children)
                .WithOne(a => a.Parent)
                .HasForeignKey(a => a.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Transaction → PgcAccount
            modelBuilder.Entity<Transact>()
                .HasOne(t => t.PgcAccount)
                .WithMany()
                .HasForeignKey(t => t.PgcAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // (Opcional) Configuraciones adicionales para Account, Transaction…
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Puedes agregar lógica extra aquí si necesitas auditar cambios, etc.
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}