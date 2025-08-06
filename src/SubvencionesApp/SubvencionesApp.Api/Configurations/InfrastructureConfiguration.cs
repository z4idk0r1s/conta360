using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;
using SubvencionesApp.Infrastructure.Repositories;
using SubvencionesApp.Infrastructure.ExternalServices;
using Polly;
using Polly.Extensions.Http;
using SubvencionesApp.Application.Interfaces;

namespace SubvencionesApp.Api.Configurations
{
    public static class InfrastructureConfiguration
    {
        public static void ConfigureInfrastructureServices(WebApplicationBuilder builder)
        {
            // Servicios externos
            builder.Services.AddScoped<IExternalSubvencionesService, ExternalSubvencionesService>();
            
            // Cliente HTTP con configuración optimizada
            builder.Services.AddHttpClient<ExternalSubvencionesService>(client =>
            {
                client.BaseAddress = new Uri("https://www.infosubvenciones.es/bdnstrans/");
                client.Timeout = TimeSpan.FromSeconds(30);
                client.DefaultRequestHeaders.Add("User-Agent", "SubvencionesApp/1.0");
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                MaxConnectionsPerServer = 10
            })
            .AddPolicyHandler(GetRetryPolicy());

            // Unit of Work y repositorios
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            RegisterRepositories(builder);
        }

        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => !msg.IsSuccessStatusCode)
                .WaitAndRetryAsync(
                    3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timespan, retryCount, context) =>
                    {
                        Console.WriteLine($"Retry {retryCount} after {timespan}ms");
                    });
        }

        public static void RegisterRepositories(WebApplicationBuilder builder)
        {
            // Registro de repositorios
            builder.Services.AddScoped<IAccionRepository, AccionRepository>();
            builder.Services.AddScoped<IAgrupacionRepository, AgrupacionRepository>();
            builder.Services.AddScoped<IAreaRepository, AreaRepository>();
            builder.Services.AddScoped<IAyudaRepository, AyudaRepository>();
            builder.Services.AddScoped<IAyudaEstadoRepository, AyudaEstadoRepository>();
            builder.Services.AddScoped<IBeneficiarioRepository, BeneficiarioRepository>();
            builder.Services.AddScoped<IConcesionRepository, ConcesionRepository>();
            builder.Services.AddScoped<IConcesionDetalleRepository, ConcesionDetalleRepository>();
            builder.Services.AddScoped<IConvocatoriaRepository, ConvocatoriaRepository>();
            builder.Services.AddScoped<IConvocatoriaDetalleRepository, ConvocatoriaDetalleRepository>();
            builder.Services.AddScoped<IDatosEstadisticosRepository, DatosEstadisticosRepository>();
            builder.Services.AddScoped<IEntidadRepository, EntidadRepository>();
            builder.Services.AddScoped<IEstadoRepository, EstadoRepository>();
            builder.Services.AddScoped<IFinalidadRepository, FinalidadRepository>();
            builder.Services.AddScoped<IFormaPagoRepository, FormaPagoRepository>();
            builder.Services.AddScoped<IGrandeBeneficiarioRepository, GrandeBeneficiarioRepository>();
            builder.Services.AddScoped<IInstrumentoRepository, InstrumentoRepository>();
            builder.Services.AddScoped<ILineaRepository, LineaRepository>();
            builder.Services.AddScoped<IMinimisRepository, MinimisRepository>();
            builder.Services.AddScoped<IMunicipioRepository, MunicipioRepository>();
            builder.Services.AddScoped<IObjetivoRepository, ObjetivoRepository>();
            builder.Services.AddScoped<IOrganismoRepository, OrganismoRepository>();
            builder.Services.AddScoped<IOrganosCodigoAdminRepository, OrganosCodigoAdminRepository>();
            builder.Services.AddScoped<IPartidoPoliticoRepository, PartidoPoliticoRepository>();
            builder.Services.AddScoped<IPlanEstrategicoRepository, PlanEstrategicoRepository>();
            builder.Services.AddScoped<IPlanEstrategicoDetalleRepository, PlanEstrategicoDetalleRepository>();
            builder.Services.AddScoped<IPlazoRepository, PlazoRepository>();
            builder.Services.AddScoped<IProgramaRepository, ProgramaRepository>();
            builder.Services.AddScoped<IProvinciaRepository, ProvinciaRepository>();
            builder.Services.AddScoped<IRegionRepository, RegionRepository>();
            builder.Services.AddScoped<IReglamentoRepository, ReglamentoRepository>();
            builder.Services.AddScoped<ISancionRepository, SancionRepository>();
            builder.Services.AddScoped<ISancionDetalleRepository, SancionDetalleRepository>();
            builder.Services.AddScoped<ISectorRepository, SectorRepository>();
            builder.Services.AddScoped<ISectorProductoRepository, SectorProductoRepository>();
            builder.Services.AddScoped<ISituacionEntornoRepository, SituacionEntornoRepository>();
            builder.Services.AddScoped<ISubtipoSubvencionRepository, SubtipoSubvencionRepository>();
            builder.Services.AddScoped<ISuscripcionRepository, SuscripcionRepository>();
            builder.Services.AddScoped<ITerceroRepository, TerceroRepository>();
            builder.Services.AddScoped<ITipoBeneficiarioRepository, TipoBeneficiarioRepository>();
            builder.Services.AddScoped<ITipoConvocatoriaRepository, TipoConvocatoriaRepository>();
            builder.Services.AddScoped<ITipoOrganismoRepository, TipoOrganismoRepository>();
            builder.Services.AddScoped<ITipoSubvencionRepository, TipoSubvencionRepository>();
            builder.Services.AddScoped<ITramoRepository, TramoRepository>();
            builder.Services.AddScoped<IUnidadAdministrativaRepository, UnidadAdministrativaRepository>();
        }
    }
}