using Microsoft.EntityFrameworkCore;
using SubvencionesApp.Core.Interfaces;
using SubvencionesApp.Core.Services;
using SubvencionesApp.Infrastructure.Api;
using SubvencionesApp.Infrastructure.Database;
using SubvencionesApp.Infrastructure.Database.Repositories;
using SubvencionesApp.Core.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos
var provider = builder.Configuration.GetValue<string>("DatabaseProvider");
var connectionString = builder.Configuration.GetConnectionString(provider + "Connection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (provider == "PostgreSql")
    {
        options.UseNpgsql(connectionString,
            b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
    }
    else
    {
        options.UseSqlite(connectionString,
            b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
    }
});

// Registro de servicios y repositorios
builder.Services.AddHttpClient<InfoSubvencionesAppClient>();
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
builder.Services.AddControllers();

var app = builder.Build();

// Configuración del pipeline
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();