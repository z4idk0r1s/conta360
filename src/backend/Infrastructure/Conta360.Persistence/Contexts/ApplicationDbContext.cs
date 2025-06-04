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

                  // Configuración de Account y Transaction (si fuera necesario)
                  modelBuilder.Entity<Account>(entity =>
                  {
                        entity.HasKey(e => e.Id);
                        entity.Property(e => e.Name)
                        .IsRequired()
                        .HasMaxLength(200);
                        // Se pueden agregar indexes, etc., si se requieren.
                  });

                  modelBuilder.Entity<Transaction>(entity =>
                  {
                        entity.HasKey(e => e.Id);
                        entity.Property(e => e.Amount).IsRequired();
                        entity.Property(e => e.Date).IsRequired();
                        entity.HasOne(e => e.Account)
                        .WithMany(a => a.Transactions)
                        .HasForeignKey(e => e.AccountId)
                        .OnDelete(DeleteBehavior.Cascade);
                  });

                  // Configuración de PgcAccount (ya existente)
                  modelBuilder.Entity<PgcAccount>(entity =>
                  {
                        entity.HasKey(e => e.Id);
                        entity.Property(e => e.Code)
                        .IsRequired()
                        .HasMaxLength(20);
                        entity.Property(e => e.Name)
                        .IsRequired()
                        .HasMaxLength(200);
                        entity.Property(e => e.Level)
                        .IsRequired();
                        entity.Property(e => e.IsMovable)
                        .IsRequired();
                        entity.Property(e => e.ParentCode)
                        .HasMaxLength(20);

                        entity.HasOne(e => e.Parent)
                        .WithMany(e => e.Children)
                        .HasPrincipalKey(e => e.Code)
                        .HasForeignKey(e => e.ParentCode)
                        .OnDelete(DeleteBehavior.Restrict);
                  });
            }
      }
}
