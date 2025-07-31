using Microsoft.EntityFrameworkCore;
using SubvencionesApp.Application.Interfaces;
using SubvencionesApp.Application.Services;
using SubvencionesApp.Application.UseCases;
using SubvencionesApp.Domain.Interfaces;
using SubvencionesApp.Infrastructure.Database;
using SubvencionesApp.Infrastructure.Repositories;
using SubvencionesApp.Infrastructure.ExternalServices;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// CONFIGURACIÓN DE LA BASE DE DATOS
// ========================================
ConfigureDatabase(builder);

// ========================================
// CONFIGURACIÓN DE SERVICIOS
// ========================================
ConfigureApplicationServices(builder);
ConfigureInfrastructureServices(builder);
ConfigureDomainServices(builder);

// ========================================
// CONFIGURACIÓN DE CONTROLADORES Y API
// ========================================
ConfigureApiServices(builder);

var app = builder.Build();

// ========================================
// PIPELINE DE LA APLICACIÓN
// ========================================
ConfigurePipeline(app);

// ========================================
// INICIALIZACIÓN DE LA BASE DE DATOS
// ========================================
await EnsureDatabaseCreated(app);

app.Run();

// ========================================
// MÉTODOS DE CONFIGURACIÓN
// ========================================

static void ConfigureDatabase(WebApplicationBuilder builder)
{
    var provider = builder.Configuration.GetValue<string>("DatabaseProvider") ?? "SQLITE";
    var connectionString = builder.Configuration.GetConnectionString($"{provider}Connection");
    
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException($"Connection string '{provider}Connection' not found.");
    }

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        switch (provider.ToUpperInvariant())
        {
            case "POSTGRESQL":
                options.UseNpgsql(connectionString, opt =>
                {
                    opt.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    opt.EnableRetryOnFailure(3);
                });
                break;            

            case "SQLITE":
            default:
                options.UseSqlite(connectionString, opt =>
                {
                    opt.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                });
                break;
        }

        // Configuraciones adicionales para optimización
        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }
    });
}

static void ConfigureApplicationServices(WebApplicationBuilder builder)
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

static void ConfigureInfrastructureServices(WebApplicationBuilder builder)
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

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
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

static void ConfigureDomainServices(WebApplicationBuilder builder)
{
    // Servicios de dominio si los hubiera
}

static void RegisterRepositories(WebApplicationBuilder builder)
{
    // Registro de repositorios
    builder.Services.AddScoped<IAccionRepository, AccionRepository>();
    builder.Services.AddScoped<IAgrupacionRepository, AgrupacionRepository>();
    builder.Services.AddScoped<IAreaRepository, AreaRepository>();
    builder.Services.AddScoped<IBeneficiarioRepository, BeneficiarioRepository>();
    builder.Services.AddScoped<IConcesionRepository, ConcesionRepository>();
    builder.Services.AddScoped<IConvocatoriaRepository, ConvocatoriaRepository>();
    builder.Services.AddScoped<IDatosEstadisticosRepository, DatosEstadisticosRepository>();
    builder.Services.AddScoped<IEntidadRepository, EntidadRepository>();
    builder.Services.AddScoped<IEstadoRepository, EstadoRepository>();
    builder.Services.AddScoped<IFormaPagoRepository, FormaPagoRepository>();
    builder.Services.AddScoped<ILineaRepository, LineaRepository>();
    builder.Services.AddScoped<IMunicipioRepository, MunicipioRepository>();
    builder.Services.AddScoped<IOrganismoRepository, OrganismoRepository>();
    builder.Services.AddScoped<IProgramaRepository, ProgramaRepository>();
    builder.Services.AddScoped<IProvinciaRepository, ProvinciaRepository>();
    builder.Services.AddScoped<ISectorRepository, SectorRepository>();
    builder.Services.AddScoped<ISituacionEntornoRepository, SituacionEntornoRepository>();
    builder.Services.AddScoped<ISubtipoSubvencionRepository, SubtipoSubvencionRepository>();
    builder.Services.AddScoped<ITipoBeneficiarioRepository, TipoBeneficiarioRepository>();
    builder.Services.AddScoped<ITipoConvocatoriaRepository, TipoConvocatoriaRepository>();
    builder.Services.AddScoped<ITipoOrganismoRepository, TipoOrganismoRepository>();
    builder.Services.AddScoped<ITipoSubvencionRepository, TipoSubvencionRepository>();
    builder.Services.AddScoped<ITramoRepository, TramoRepository>();
    builder.Services.AddScoped<IUnidadAdministrativaRepository, UnidadAdministrativaRepository>();
}

static void ConfigureApiServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers(options =>
    {
        options.SuppressAsyncSuffixInActionNames = false;
    }).ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = false;
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo 
        { 
            Title = "Subvenciones API", 
            Version = "v1",
            Description = "API para la gestión de subvenciones del BDNS",
            Contact = new OpenApiContact
            {
                Name = "Equipo de Desarrollo",
                Email = "desarrollo@subvenciones.es"
            }
        });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }
    });

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });

    builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
    });

    builder.Services.AddMemoryCache();
    
    builder.Services.AddHealthChecks()
        .AddDbContext<AppDbContext>();
}

static void ConfigurePipeline(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Subvenciones API V1");
            c.RoutePrefix = string.Empty;
        });
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseResponseCompression();
    app.UseHttpsRedirection();
    app.UseCors();
    
    app.UseRouting();
    app.UseAuthorization();
    
    app.MapControllers();
    app.MapHealthChecks("/health");

    app.MapGet("/info", () => new
    {
        Application = "SubvencionesApp",
        Version = "1.0.0",
        Environment = app.Environment.EnvironmentName,
        Timestamp = DateTime.UtcNow
    });
}

static async Task EnsureDatabaseCreated(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    try
    {
        if (app.Environment.IsDevelopment())
        {
            await context.Database.EnsureCreatedAsync();
        }
        else
        {
            await context.Database.EnsureCreatedAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error durante la inicialización de la base de datos");
        throw;
    }
}