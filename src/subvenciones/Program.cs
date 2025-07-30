using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Core.Services;
using SubvencionesApp.Infrastructure.Api;
using SubvencionesApp.Infrastructure.Database;
using SubvencionesApp.Infrastructure.Database.Repositories;
using SubvencionesApp.Core.Entities;

var builder = Host.CreateApplicationBuilder(args);

// Leer la configuración del proveedor y la cadena de conexión
var provider = builder.Configuration.GetValue<string>("DatabaseProvider");
var connectionString = builder.Configuration.GetConnectionString(provider + "Connection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (provider == "PostgreSql")
    {
        options.UseNpgsql(connectionString,
            b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
    }
    else // Valor por defecto si el proveedor no es PostgreSQL
    {
        options.UseSqlite(connectionString,
            b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
    }
});

// Registrar los servicios y repositorios
builder.Services.AddHttpClient<InfoSubvencionesApiClient>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISubvencionesService, SubvencionesService>();
builder.Services.AddScoped<IAccionRepository, AccionRepository>();
builder.Services.AddScoped<IAgrupacionRepository, AgrupacionRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IBeneficiarioRepository, BeneficiarioRepository>();
builder.Services.AddScoped<IConcesionRepository, ConcesionRepository>();
builder.Services.AddScoped<IConvocatoriaRepository, ConvocatoriaRepository>();
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


var app = builder.Build();

app.Run();