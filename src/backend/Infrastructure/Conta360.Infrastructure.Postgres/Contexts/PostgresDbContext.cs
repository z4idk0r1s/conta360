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

            // Claves primarias
            modelBuilder.Entity<Account>().HasKey(a => a.Id);
            modelBuilder.Entity<Transact>().HasKey(t => t.Id);
            modelBuilder.Entity<PgcAccount>().HasKey(p => p.Id);

            // Índice único para el código de cuenta PGC
            modelBuilder.Entity<PgcAccount>()
                .HasIndex(a => a.Code)
                .IsUnique();

            // Relación padre-hijo para cuentas PGC
            modelBuilder.Entity<PgcAccount>()
                .HasMany(p => p.Children)
                .WithOne(p => p.Parent)
                .HasForeignKey(p => p.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Transact → PgcAccount
            modelBuilder.Entity<Transact>()
                .HasOne(t => t.PgcAccount)
                .WithMany()
                .HasForeignKey(t => t.PgcAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Transact → Account (¡esto es lo que evita el warning!)
            modelBuilder.Entity<Transact>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
