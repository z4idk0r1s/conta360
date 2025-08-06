using Microsoft.EntityFrameworkCore;
using SubvencionesApp.Domain.Entities;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;

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
        public DbSet<Ayuda> Ayudas { get; set; } = null!;
        public DbSet<AyudaEstado> AyudasEstados { get; set; } = null!;
        public DbSet<Beneficiario> Beneficiarios { get; set; } = null!;
        public DbSet<Concesion> Concesiones { get; set; } = null!;
        public DbSet<ConcesionDetalle> ConcesionesDetalle { get; set; } = null!;
        public DbSet<ConfiguracionMicroportal> ConfiguracionesMicroportal { get; set; } = null!;
        public DbSet<Convocatoria> Convocatorias { get; set; } = null!;
        public DbSet<ConvocatoriaDetalle> ConvocatoriasDetalle { get; set; } = null!;
        public DbSet<DatosEstadisticos> DatosEstadisticos { get; set; } = null!;
        public DbSet<EnlaceMicroVentana> EnlacesMicroVentana { get; set; } = null!;
        public DbSet<Entidad> Entidades { get; set; } = null!;
        public DbSet<Estado> Estados { get; set; } = null!;
        public DbSet<Finalidad> Finalidades { get; set; } = null!;
        public DbSet<FormaPago> FormasPago { get; set; } = null!;
        public DbSet<GrandeBeneficiario> GrandesBeneficiarios { get; set; } = null!;
        public DbSet<Instrumento> Instrumentos { get; set; } = null!;
        public DbSet<Linea> Lineas { get; set; } = null!;
        public DbSet<Minimis> Minimis { get; set; } = null!;
        public DbSet<Municipio> Municipios { get; set; } = null!;
        public DbSet<Objetivo> Objetivos { get; set; } = null!;
        public DbSet<Organismo> Organismos { get; set; } = null!;
        public DbSet<OrganosCodigoAdmin> OrganosCodigoAdmin { get; set; } = null!;
        public DbSet<PartidoPolitico> PartidosPoliticos { get; set; } = null!;
        public DbSet<PlanEstrategico> PlanesEstrategicos { get; set; } = null!;
        public DbSet<PlanEstrategicoDetalle> PlanesEstrategicosDetalle { get; set; } = null!;
        public DbSet<Plazo> Plazos { get; set; } = null!;
        public DbSet<Programa> Programas { get; set; } = null!;
        public DbSet<Provincia> Provincias { get; set; } = null!;
        public DbSet<Region> Regiones { get; set; } = null!;
        public DbSet<Reglamento> Reglamentos { get; set; } = null!;
        public DbSet<Sancion> Sanciones { get; set; } = null!;
        public DbSet<SancionDetalle> SancionesDetalle { get; set; } = null!;
        public DbSet<Sector> Sectores { get; set; } = null!;
        public DbSet<SectorProducto> SectoresProductos { get; set; } = null!;
        public DbSet<SituacionEntorno> SituacionesEntorno { get; set; } = null!;
        public DbSet<SubtipoSubvencion> SubtiposSubvencion { get; set; } = null!;
        public DbSet<Suscripcion> Suscripciones { get; set; } = null!;
        public DbSet<Tercero> Terceros { get; set; } = null!;
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

            // Configuración para Beneficiario (añadiendo soporte para SQLite y PostgreSQL)
            modelBuilder.Entity<Beneficiario>(entity =>
            {
                entity.HasKey(e => e.Id);

                if (Database.IsNpgsql()) // PostgreSQL
                {
                    entity.Property(e => e.Id)
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v1()")
                        .ValueGeneratedOnAdd();
                }
                else // SQLite
                {
                    entity.Property(e => e.Id)
                        .HasConversion(
                            v => v.ToString(),
                            v => Guid.Parse(v))
                        .HasColumnType("TEXT")
                        .ValueGeneratedOnAdd();
                }

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
                typeof(UnidadAdministrativa), typeof(Finalidad), typeof(Instrumento),
                typeof(Region), typeof(Reglamento), typeof(SectorProducto), typeof(Actividad)
            };

            foreach (var entityType in catalogEntities)
            {
                modelBuilder.Entity(entityType, entity =>
                {
                    entity.HasKey("Id");
                    entity.Property("Descripcion").HasMaxLength(500).IsRequired();

                    if (entityType.GetProperty("Id")?.PropertyType == typeof(Guid))
                    {
                        if (Database.IsNpgsql())
                        {
                            entity.Property<Guid>("Id")
                                .HasColumnType("uuid")
                                .HasDefaultValueSql("uuid_generate_v1()")
                                .ValueGeneratedOnAdd();
                        }
                        else
                        {
                            entity.Property<Guid>("Id")
                                .HasConversion(
                                    v => v.ToString(),
                                    v => Guid.Parse(v))
                                .HasColumnType("TEXT")
                                .ValueGeneratedOnAdd();
                        }
                    }
                });
            }

            // Configuraciones específicas para entidades con campos adicionales
            modelBuilder.Entity<Linea>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Codigo).HasMaxLength(50);
                entity.Property(e => e.Nombre).HasMaxLength(500).IsRequired();
            });
            
            // Nuevas configuraciones para las entidades de la API Oficial
            modelBuilder.Entity<Ayuda>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExternalId).IsRequired();
                entity.HasIndex(e => e.ExternalId).IsUnique();
            });

            modelBuilder.Entity<AyudaEstado>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExternalId).IsRequired();
                entity.HasIndex(e => e.ExternalId).IsUnique();
            });

            modelBuilder.Entity<ConvocatoriaDetalle>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExternalId).IsRequired();
                entity.HasIndex(e => e.ExternalId).IsUnique();
            });

            modelBuilder.Entity<ConcesionDetalle>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExternalId).IsRequired();
                entity.HasIndex(e => e.ExternalId).IsUnique();
            });

            modelBuilder.Entity<Minimis>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExternalId).IsRequired();
                entity.HasIndex(e => e.ExternalId).IsUnique();
            });
            
            modelBuilder.Entity<GrandeBeneficiario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExternalId).IsRequired();
                entity.HasIndex(e => e.ExternalId).IsUnique();
            });
            
            modelBuilder.Entity<PartidoPolitico>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExternalId).IsRequired();
                entity.HasIndex(e => e.ExternalId).IsUnique();
            });
            
            modelBuilder.Entity<PlanEstrategico>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExternalId).IsRequired();
                entity.HasIndex(e => e.ExternalId).IsUnique();
            });
            
            modelBuilder.Entity<PlanEstrategicoDetalle>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExternalId).IsRequired();
                entity.HasIndex(e => e.ExternalId).IsUnique();
            });

            modelBuilder.Entity<Plazo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExternalId).IsRequired();
                entity.HasIndex(e => e.ExternalId).IsUnique();
            });

            modelBuilder.Entity<Sancion>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExternalId).IsRequired();
                entity.HasIndex(e => e.ExternalId).IsUnique();
            });
            
            modelBuilder.Entity<SancionDetalle>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExternalId).IsRequired();
                entity.HasIndex(e => e.ExternalId).IsUnique();
            });
            
            modelBuilder.Entity<Tercero>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExternalId).IsRequired();
                entity.HasIndex(e => e.ExternalId).IsUnique();
            });

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

            // Nuevos índices para las entidades de la API Oficial
            modelBuilder.Entity<Ayuda>()
                .HasIndex(e => e.ExternalId)
                .IsUnique()
                .HasDatabaseName("IX_Ayudas_ExternalId");

            modelBuilder.Entity<AyudaEstado>()
                .HasIndex(e => e.ExternalId)
                .IsUnique()
                .HasDatabaseName("IX_AyudasEstados_ExternalId");

            modelBuilder.Entity<ConvocatoriaDetalle>()
                .HasIndex(e => e.ExternalId)
                .IsUnique()
                .HasDatabaseName("IX_ConvocatoriasDetalle_ExternalId");

            modelBuilder.Entity<ConcesionDetalle>()
                .HasIndex(e => e.ExternalId)
                .IsUnique()
                .HasDatabaseName("IX_ConcesionesDetalle_ExternalId");

            modelBuilder.Entity<Minimis>()
                .HasIndex(e => e.ExternalId)
                .IsUnique()
                .HasDatabaseName("IX_Minimis_ExternalId");
            
            modelBuilder.Entity<GrandeBeneficiario>()
                .HasIndex(e => e.ExternalId)
                .IsUnique()
                .HasDatabaseName("IX_GrandesBeneficiarios_ExternalId");
            
            modelBuilder.Entity<PartidoPolitico>()
                .HasIndex(e => e.ExternalId)
                .IsUnique()
                .HasDatabaseName("IX_PartidosPoliticos_ExternalId");
            
            modelBuilder.Entity<PlanEstrategico>()
                .HasIndex(e => e.ExternalId)
                .IsUnique()
                .HasDatabaseName("IX_PlanesEstrategicos_ExternalId");
            
            modelBuilder.Entity<PlanEstrategicoDetalle>()
                .HasIndex(e => e.ExternalId)
                .IsUnique()
                .HasDatabaseName("IX_PlanesEstrategicosDetalle_ExternalId");

            modelBuilder.Entity<Plazo>()
                .HasIndex(e => e.ExternalId)
                .IsUnique()
                .HasDatabaseName("IX_Plazos_ExternalId");

            modelBuilder.Entity<Sancion>()
                .HasIndex(e => e.ExternalId)
                .IsUnique()
                .HasDatabaseName("IX_Sanciones_ExternalId");
            
            modelBuilder.Entity<SancionDetalle>()
                .HasIndex(e => e.ExternalId)
                .IsUnique()
                .HasDatabaseName("IX_SancionesDetalle_ExternalId");
            
            modelBuilder.Entity<Tercero>()
                .HasIndex(e => e.ExternalId)
                .IsUnique()
                .HasDatabaseName("IX_Terceros_ExternalId");

            modelBuilder.Entity<EnlaceMicroVentana>()
                .HasIndex(e => e.ExternalId)
                .IsUnique()
                .HasDatabaseName("IX_EnlacesMicroVentana_ExternalId");
            
            modelBuilder.Entity<Suscripcion>()
                .HasIndex(e => e.ExternalId)
                .IsUnique()
                .HasDatabaseName("IX_Suscripciones_ExternalId");
        }

        private void ConfigureRelaciones(ModelBuilder modelBuilder)
        {
            // Relaciones para Convocatoria
            modelBuilder.Entity<Convocatoria>()
                .HasOne(e => e.TipoConvocatoria)
                .WithMany(tc => tc.Convocatorias)
                .HasForeignKey(e => e.TipoConvocatoriaId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Convocatoria>()
                .HasOne(e => e.TipoSubvencion)
                .WithMany(ts => ts.Convocatorias)
                .HasForeignKey(e => e.TipoSubvencionId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Convocatoria>()
                .HasOne(e => e.Organismo)
                .WithMany(o => o.Convocatorias)
                .HasForeignKey(e => e.OrganismoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Convocatoria>()
                .HasOne(e => e.SituacionEntorno)
                .WithMany(se => se.Convocatorias)
                .HasForeignKey(e => e.SituacionEntornoId)
                .OnDelete(DeleteBehavior.SetNull);

            // Relaciones para Concesion
            modelBuilder.Entity<Concesion>()
                .HasOne(e => e.Beneficiario)
                .WithMany(b => b.Concesiones)
                .HasForeignKey(e => e.BeneficiarioId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Concesion>()
                .HasOne(e => e.Convocatoria)
                .WithMany(c => c.Concesiones)
                .HasForeignKey(e => e.ConvocatoriaId)
                .OnDelete(DeleteBehavior.SetNull);
            
            // Relaciones para ConcesionDetalle (corregidas)
            modelBuilder.Entity<ConcesionDetalle>()
                .HasOne(e => e.Beneficiario)
                .WithMany(b => b.ConcesionesDetalle)
                .HasForeignKey(e => e.BeneficiarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ConcesionDetalle>()
                .HasOne(e => e.Organismo)
                .WithMany(o => o.ConcesionesDetalle)
                .HasForeignKey(e => e.OrganismoId)
                .OnDelete(DeleteBehavior.Cascade);
        }


        // Método para configurar el comportamiento de eliminación suave si se requiere
        public override int SaveChanges()
        {
            GenerateGuidsForNewEntities();
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            GenerateGuidsForNewEntities();
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }
        
        /// <summary>
        /// Asegura que todas las nuevas entidades con una propiedad 'Id' de tipo Guid
        /// tengan un valor generado si aún no tienen uno. Esto es vital para bases de datos como SQLite
        /// donde la generación automática de GUIDs por parte de EF Core no siempre es fiable.
        /// </summary>
        private void GenerateGuidsForNewEntities()
        {
            var entriesWithGuid = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added && e.Entity.GetType().GetProperty("Id")?.PropertyType == typeof(Guid));

            foreach (var entry in entriesWithGuid)
            {
                var idProperty = entry.Entity.GetType().GetProperty("Id");
                if (idProperty?.GetValue(entry.Entity) is Guid currentGuid && currentGuid == Guid.Empty)
                {
                    idProperty.SetValue(entry.Entity, Guid.NewGuid());
                }
            }
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
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