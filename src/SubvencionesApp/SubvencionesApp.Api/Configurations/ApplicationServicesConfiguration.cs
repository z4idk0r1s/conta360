using SubvencionesApp.Application.Interfaces;
using SubvencionesApp.Application.Services;
using SubvencionesApp.Application.UseCases;

namespace SubvencionesApp.Api.Configurations
{
    public static class ApplicationServicesConfiguration
    {
        public static void ConfigureApplicationServices(WebApplicationBuilder builder)
        {
            // Servicios de aplicación principales
            builder.Services.AddScoped<ISubvencionSyncService, SubvencionSyncService>();
            builder.Services.AddScoped<ISubvencionQueryService, SubvencionQueryService>();
            
            // Servicios específicos por entidad
            builder.Services.AddScoped<ConvocatoriaService>();
            builder.Services.AddScoped<ConcesionService>();
            builder.Services.AddScoped<BeneficiarioService>();
            builder.Services.AddScoped<AccionService>();
            builder.Services.AddScoped<AgrupacionService>();
            builder.Services.AddScoped<AreaService>();
            builder.Services.AddScoped<DatosEstadisticosService>();
            builder.Services.AddScoped<EntidadService>();
            builder.Services.AddScoped<EstadoService>();
            builder.Services.AddScoped<FormaPagoService>();
            builder.Services.AddScoped<LineaService>();
            builder.Services.AddScoped<MunicipioService>();
            builder.Services.AddScoped<OrganismoService>();
            builder.Services.AddScoped<ProgramaService>();
            builder.Services.AddScoped<ProvinciaService>();
            builder.Services.AddScoped<SectorService>();
            builder.Services.AddScoped<SituacionEntornoService>();
            builder.Services.AddScoped<SubtipoSubvencionService>();
            builder.Services.AddScoped<TipoBeneficiarioService>();
            builder.Services.AddScoped<TipoConvocatoriaService>();
            builder.Services.AddScoped<TipoOrganismoService>();
            builder.Services.AddScoped<TipoSubvencionService>();
            builder.Services.AddScoped<TramoService>();
            builder.Services.AddScoped<UnidadAdministrativaService>();
        }
    }
}