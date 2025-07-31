using Microsoft.EntityFrameworkCore;
using SubvencionesApp.Core.Entities;

namespace SubvencionesApp.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Accion> Acciones { get; set; }
        public DbSet<Agrupacion> Agrupaciones { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Beneficiario> Beneficiarios { get; set; }
        public DbSet<Concesion> Concesiones { get; set; }
        public DbSet<Convocatoria> Convocatorias { get; set; }
        public DbSet<Entidad> Entidades { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<FormaPago> FormasPago { get; set; }
        public DbSet<Linea> Lineas { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Organismo> Organismos { get; set; }
        public DbSet<Programa> Programas { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Sector> Sectores { get; set; }
        public DbSet<SituacionEntorno> SituacionesEntorno { get; set; }
        public DbSet<SubtipoSubvencion> SubtiposSubvencion { get; set; }
        public DbSet<TipoBeneficiario> TiposBeneficiario { get; set; }
        public DbSet<TipoConvocatoria> TiposConvocatoria { get; set; }
        public DbSet<TipoOrganismo> TiposOrganismo { get; set; }
        public DbSet<TipoSubvencion> TiposSubvencion { get; set; }
        public DbSet<Tramo> Tramos { get; set; }
        public DbSet<UnidadAdministrativa> UnidadesAdministrativas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Concesion>()
                .HasOne(c => c.Beneficiario)
                .WithMany(b => b.Concesiones)
                .HasForeignKey(c => c.BeneficiarioId);

            modelBuilder.Entity<Concesion>()
                .HasOne(c => c.Convocatoria)
                .WithMany(c => c.Concesiones)
                .HasForeignKey(c => c.ConvocatoriaId);

            modelBuilder.Entity<Convocatoria>()
                .HasOne(c => c.TipoConvocatoria)
                .WithMany()
                .HasForeignKey(c => c.TipoConvocatoriaId);

            modelBuilder.Entity<Convocatoria>()
                .HasOne(c => c.TipoSubvencion)
                .WithMany()
                .HasForeignKey(c => c.TipoSubvencionId);

            modelBuilder.Entity<Convocatoria>()
                .HasOne(c => c.Organismo)
                .WithMany()
                .HasForeignKey(c => c.OrganismoId);

            modelBuilder.Entity<Convocatoria>()
                .HasOne(c => c.SituacionEntorno)
                .WithMany()
                .HasForeignKey(c => c.SituacionEntornoId);
        }
    }
}