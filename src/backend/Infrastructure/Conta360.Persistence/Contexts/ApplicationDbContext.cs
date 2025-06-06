using Microsoft.EntityFrameworkCore;
using Conta360.Domain.Entities;
using Conta360.Core.Interfaces;

namespace Conta360.Persistence.Contexts
{
      /// <summary>
      /// Implementación concreta del DbContext que persiste entidades de dominio
      /// en SQLite (o cualquier otro proveedor EF Core).
      /// </summary>
      public class ApplicationDbContext : DbContext, IApplicationDbContext
      {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            /// <inheritdoc/>
            public DbSet<Account> Accounts { get; set; }

            /// <inheritdoc/>
            public DbSet<Transaction> Transactions { get; set; }

            public DbSet<PgcAccount> PgcAccounts { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                  base.OnModelCreating(modelBuilder);

                  modelBuilder.Entity<PgcAccount>()
                        .HasIndex(a => a.Code)
                        .IsUnique();

                  modelBuilder.Entity<PgcAccount>()
                        .HasMany(a => a.Children)
                        .WithOne(a => a.Parent)
                        .HasForeignKey(a => a.ParentId)
                        .OnDelete(DeleteBehavior.Restrict);

                  modelBuilder.Entity<Transaction>()
                        .HasOne(t => t.PgcAccount)
                        .WithMany()
                        .HasForeignKey(t => t.PgcAccountId)
                        .OnDelete(DeleteBehavior.Cascade);
            }
      }
}
