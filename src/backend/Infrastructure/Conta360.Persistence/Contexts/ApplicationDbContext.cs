using Microsoft.EntityFrameworkCore;
using Conta360.Domain.Entities;
using Conta360.Core.Interfaces;

namespace Conta360.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Otros DbSet existentes...
        public DbSet<PgcAccount> PgcAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
