using SubvencionesApp.Application.Interfaces;
using SubvencionesApp.Application.Services;
using SubvencionesApp.Application.UseCases;
using SubvencionesApp.Infrastructure.ExternalServices; 
using AutoMapper;

namespace SubvencionesApp.Api.Configurations
{
    public static class ApplicationServicesConfiguration
    {
        public static void ConfigureApplicationServices(WebApplicationBuilder builder)
        {
            // AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            
            // Servicios de aplicación principales
            builder.Services.AddScoped<ISubvencionSyncService, SubvencionSyncService>();
            builder.Services.AddScoped<ISubvencionQueryService, SubvencionQueryService>();
            
            // Servicios específicos por entidad
            builder.Services.AddScoped<AccionService>();
            builder.Services.AddScoped<AgrupacionService>();
            builder.Services.AddScoped<AreaService>();
            builder.Services.AddScoped<AyudaService>();
            builder.Services.AddScoped<AyudaEstadoService>();
            builder.Services.AddScoped<BeneficiarioService>();
            builder.Services.AddScoped<ConcesionService>();
            builder.Services.AddScoped<ConcesionDetalleService>();
            builder.Services.AddScoped<ConvocatoriaService>();
            builder.Services.AddScoped<ConvocatoriaDetalleService>();
            builder.Services.AddScoped<DatosEstadisticosService>();
            builder.Services.AddScoped<EntidadService>();
            builder.Services.AddScoped<EstadoService>();
            builder.Services.AddScoped<FinalidadService>();
            builder.Services.AddScoped<FormaPagoService>();
            builder.Services.AddScoped<GrandeBeneficiarioService>();
            builder.Services.AddScoped<InstrumentoService>();
            builder.Services.AddScoped<LineaService>();
            builder.Services.AddScoped<MinimisService>();
            builder.Services.AddScoped<MunicipioService>();
            builder.Services.AddScoped<ObjetivoService>();
            builder.Services.AddScoped<OrganismoService>();
            builder.Services.AddScoped<OrganosCodigoAdminService>();
            builder.Services.AddScoped<PartidoPoliticoService>();
            builder.Services.AddScoped<PlanEstrategicoService>();
            builder.Services.AddScoped<PlanEstrategicoDetalleService>();
            builder.Services.AddScoped<PlazoService>();
            builder.Services.AddScoped<ProgramaService>();
            builder.Services.AddScoped<ProvinciaService>();
            builder.Services.AddScoped<RegionService>();
            builder.Services.AddScoped<ReglamentoService>();
            builder.Services.AddScoped<SancionService>();
            builder.Services.AddScoped<SancionDetalleService>();
            builder.Services.AddScoped<SectorService>();
            builder.Services.AddScoped<SectorProductoService>();
            builder.Services.AddScoped<SituacionEntornoService>();
            builder.Services.AddScoped<SubtipoSubvencionService>();
            builder.Services.AddScoped<SuscripcionService>();
            builder.Services.AddScoped<TerceroService>();
            builder.Services.AddScoped<TipoBeneficiarioService>();
            builder.Services.AddScoped<TipoConvocatoriaService>();
            builder.Services.AddScoped<TipoOrganismoService>();
            builder.Services.AddScoped<TipoSubvencionService>();
            builder.Services.AddScoped<TramoService>();
            builder.Services.AddScoped<UnidadAdministrativaService>();
        }
    }
}