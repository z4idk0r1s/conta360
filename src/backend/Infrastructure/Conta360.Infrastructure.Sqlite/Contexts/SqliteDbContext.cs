using Conta360.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Conta360.Application.Interfaces;

namespace Conta360.Infrastructure.Sqlite.Contexts
{
    /// <summary>
    /// Implementación de IApplicationDbContext usando SQLite.
    /// </summary>
    public class SqliteDbContext : DbContext, IApplicationDbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options)
            : base(options)
        {
        }

        // Estos miembros deben existir para cumplir IApplicationDbContext:
        public DbSet<PgcAccount> PgcAccounts { get; set; }
        public DbSet<Transact> Transactions { get; set; }
        public DbSet<Account> Accounts { get; set; }

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

            // (Opcional) Configuraciones adicionales para Account, Transaction….
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
