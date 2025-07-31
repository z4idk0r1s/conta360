using Microsoft.EntityFrameworkCore;
using SubvencionesApp.Domain.Entities;
using System.Reflection;

namespace SubvencionesApp.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets para todas las entidades
        public DbSet<Accion> Acciones { get; set; } = null!;
        public DbSet<Agrupacion> Agrupaciones { get; set; } = null!;
        public DbSet<Area> Areas { get; set; } = null!;
        public DbSet<Beneficiario> Beneficiarios { get; set; } = null!;
        public DbSet<Concesion> Concesiones { get; set; } = null!;
        public DbSet<Convocatoria> Convocatorias { get; set; } = null!;
        public DbSet<DatosEstadisticos> DatosEstadisticos { get; set; } = null!;
        public DbSet<Entidad> Entidades { get; set; } = null!;
        public DbSet<Estado> Estados { get; set; } = null!;
        public DbSet<FormaPago> FormasPago { get; set; } = null!;
        public DbSet<Linea> Lineas { get; set; } = null!;
        public DbSet<Municipio> Municipios { get; set; } = null!;
        public DbSet<Organismo> Organismos { get; set; } = null!;
        public DbSet<Programa> Programas { get; set; } = null!;
        public DbSet<Provincia> Provincias { get; set; } = null!;
        public DbSet<Sector> Sectores { get; set; } = null!;
        public DbSet<SituacionEntorno> SituacionesEntorno { get; set; } = null!;
        public DbSet<SubtipoSubvencion> SubtiposSubvencion { get; set; } = null!;
        public DbSet<TipoBeneficiario> TiposBeneficiario { get; set; } = null!;
        public DbSet<TipoConvocatoria> TiposConvocatoria { get; set; } = null!;
        public DbSet<TipoOrganismo> TiposOrganismo { get; set; } = null!;
        public DbSet<TipoSubvencion> TiposSubvencion { get; set; } = null!;
        public DbSet<Tramo> Tramos { get; set; } = null!;
        public DbSet<UnidadAdministrativa> UnidadesAdministrativas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar todas las configuraciones de entidades automáticamente
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Configuraciones específicas que no están en archivos separados
            ConfigureEntidades(modelBuilder);
            ConfigureIndices(modelBuilder);
            ConfigureRelaciones(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            
            // Configuraciones adicionales de rendimiento
            optionsBuilder.EnableServiceProviderCaching();
            optionsBuilder.EnableSensitiveDataLogging(false); // Solo en desarrollo
        }

        private void ConfigureEntidades(ModelBuilder modelBuilder)
        {
            // Configuración para Convocatoria
            modelBuilder.Entity<Convocatoria>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Objeto).HasMaxLength(1000);
                entity.Property(e => e.Extracto).HasMaxLength(2000);
                entity.Property(e => e.Enlace).HasMaxLength(500);
                entity.Property(e => e.ReferenciaBDNS).HasMaxLength(50);
                entity.Property(e => e.Ejercicio).IsRequired();
                entity.Property(e => e.FechaPublicacion).IsRequired();
                
                // Configuración de precision para decimales si los hay
                // entity.Property(e => e.Importe).HasPrecision(18, 2);
            });

            // Configuración para Concesion
            modelBuilder.Entity<Concesion>(entity =>
            {
                entity.HasKey(e => e.IdConcesion);
                entity.Property(e => e.ReferenciaBDNS).HasMaxLength(50);
                entity.Property(e => e.ReferenciaPublicacion).HasMaxLength(100);
                entity.Property(e => e.Importe).HasPrecision(18, 2);
                entity.Property(e => e.Ejercicio).IsRequired();
                entity.Property(e => e.FechaConcesion).IsRequired();
            });

            // Configuración para Beneficiario
            modelBuilder.Entity<Beneficiario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).HasMaxLength(500).IsRequired();
                entity.Property(e => e.Identificacion).HasMaxLength(50);
            });

            // Configuraciones para entidades de catálogo (maestros)
            var catalogEntities = new[]
            {
                typeof(Accion), typeof(Agrupacion), typeof(Area), typeof(Entidad),
                typeof(Estado), typeof(FormaPago), typeof(Municipio), typeof(Organismo),
                typeof(Provincia), typeof(Sector), typeof(SituacionEntorno),
                typeof(SubtipoSubvencion), typeof(TipoBeneficiario), typeof(TipoConvocatoria),
                typeof(TipoOrganismo), typeof(TipoSubvencion), typeof(Tramo),
                typeof(UnidadAdministrativa)
            };

            foreach (var entityType in catalogEntities)
            {
                modelBuilder.Entity(entityType, entity =>
                {
                    entity.HasKey("Id");
                    entity.Property("Descripcion").HasMaxLength(500).IsRequired();
                });
            }

            // Configuraciones específicas para entidades con campos adicionales
            modelBuilder.Entity<Linea>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Codigo).HasMaxLength(50);
                entity.Property(e => e.Nombre).HasMaxLength(500).IsRequired();
            });

            modelBuilder.Entity<Programa>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Codigo).HasMaxLength(50);
                entity.Property(e => e.Descripcion).HasMaxLength(500).IsRequired();
            });

            modelBuilder.Entity<DatosEstadisticos>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Descripcion).HasMaxLength(500).IsRequired();
                entity.Property(e => e.TotalConcesiones).IsRequired();
                entity.Property(e => e.ImporteTotal).HasPrecision(18, 2);
            });
        }

        private void ConfigureIndices(ModelBuilder modelBuilder)
        {
            // Índices para optimizar consultas frecuentes
            
            // Convocatorias
            modelBuilder.Entity<Convocatoria>()
                .HasIndex(e => e.Ejercicio)
                .HasDatabaseName("IX_Convocatorias_Ejercicio");
            
            modelBuilder.Entity<Convocatoria>()
                .HasIndex(e => e.FechaPublicacion)
                .HasDatabaseName("IX_Convocatorias_FechaPublicacion");
            
            modelBuilder.Entity<Convocatoria>()
                .HasIndex(e => e.ReferenciaBDNS)
                .HasDatabaseName("IX_Convocatorias_ReferenciaBDNS");

            // Concesiones
            modelBuilder.Entity<Concesion>()
                .HasIndex(e => e.Ejercicio)
                .HasDatabaseName("IX_Concesiones_Ejercicio");
            
            modelBuilder.Entity<Concesion>()
                .HasIndex(e => e.FechaConcesion)
                .HasDatabaseName("IX_Concesiones_FechaConcesion");
            
            modelBuilder.Entity<Concesion>()
                .HasIndex(e => e.BeneficiarioId)
                .HasDatabaseName("IX_Concesiones_BeneficiarioId");
            
            modelBuilder.Entity<Concesion>()
                .HasIndex(e => e.ConvocatoriaId)
                .HasDatabaseName("IX_Concesiones_ConvocatoriaId");

            // Beneficiarios
            modelBuilder.Entity<Beneficiario>()
                .HasIndex(e => e.Identificacion)
                .IsUnique()
                .HasDatabaseName("IX_Beneficiarios_Identificacion");
            
            modelBuilder.Entity<Beneficiario>()
                .HasIndex(e => e.Nombre)
                .HasDatabaseName("IX_Beneficiarios_Nombre");
        }

        private void ConfigureRelaciones(ModelBuilder modelBuilder)
        {
            // Relaciones para Convocatoria
            modelBuilder.Entity<Convocatoria>()
                .HasOne<TipoConvocatoria>()
                .WithMany()
                .HasForeignKey(e => e.TipoConvocatoriaId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Convocatoria>()
                .HasOne<TipoSubvencion>()
                .WithMany()
                .HasForeignKey(e => e.TipoSubvencionId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Convocatoria>()
                .HasOne<Organismo>()
                .WithMany()
                .HasForeignKey(e => e.OrganismoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Convocatoria>()
                .HasOne<SituacionEntorno>()
                .WithMany()
                .HasForeignKey(e => e.SituacionEntornoId)
                .OnDelete(DeleteBehavior.SetNull);

            // Relaciones para Concesion
            modelBuilder.Entity<Concesion>()
                .HasOne<Beneficiario>()
                .WithMany()
                .HasForeignKey(e => e.BeneficiarioId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Concesion>()
                .HasOne<Convocatoria>()
                .WithMany()
                .HasForeignKey(e => e.ConvocatoriaId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        // Método para configurar el comportamiento de eliminación suave si se requiere
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                // Si las entidades tienen campos de auditoría, actualizarlos aquí
                if (entry.Entity is IAuditableEntity auditableEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        auditableEntity.CreatedAt = DateTime.UtcNow;
                    }
                    auditableEntity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }

    // Interface para entidades auditables (opcional)
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}