using Conta360.Application.Features.Accounts.Commands.CreateAccount;
using Conta360.Application.Features.Accounts.Commands.CreateAccount.Queries;
using Conta360.CrossCutting.IoC;
using MediatR;
using Microsoft.OpenApi.Models;
using Serilog;
using Conta360.Presentation.Api.Models;
using Conta360.Application.Interfaces; // Necesario para IApplicationDbContext
using Microsoft.EntityFrameworkCore;    // Necesario para Database.Migrate()
using Microsoft.Extensions.Logging;     // Necesario para ILoggerFactory y LoggingBuilder
using Conta360.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Logging profesional con Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));

// --- INICIO: CÓDIGO DE DIAGNÓSTICO DE CONFIGURACIÓN PGC ---
// Este código es temporal para depuración y debe eliminarse después de resolver el problema.
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});
var logger = LoggerFactory.Create(config => config.AddConsole()).CreateLogger<Program>();

var pgcSection = builder.Configuration.GetSection("Pgc");
var extractDirectoryValue = pgcSection["ExtractDirectory"];
var taxonomyZipUrlValue = pgcSection["TaxonomyZipUrl"]; // También diagnosticamos este

logger.LogInformation("DIAGNÓSTICO CONFIGURACIÓN PGC:");
logger.LogInformation("  Sección 'Pgc' existe: {PgcSectionExists}", pgcSection != null);
if (pgcSection != null)
{
    logger.LogInformation("  ExtractDirectory desde IConfiguration: '{ExtractDirectory}'", extractDirectoryValue);
    logger.LogInformation("  TaxonomyZipUrl desde IConfiguration: '{TaxonomyZipUrl}'", taxonomyZipUrlValue);
}
// --- FIN: CÓDIGO DE DIAGNÓSTICO DE CONFIGURACIÓN PGC ---


// Servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Conta360 API", Version = "v1" });
});

// Registro de dependencias de la aplicación e infraestructura
builder.Services
    .AddConta360Application(builder.Configuration)
    .AddConta360Infrastructure(builder.Configuration, dbProvider: "Sqlite"); // o "Postgres"

var app = builder.Build();

// --- INICIO: Gestión de Migraciones de EF Core (Condicional) ---
// La migración automática se ejecuta si:
// 1. El entorno es 'Development'.
// O
// 2. La variable de configuración 'EFCORE_AUTOMIGRATE' está explícitamente establecida a 'true'.
// Esto asegura migraciones automáticas en desarrollo y control manual en producción.
var autoMigrateEnabled = builder.Configuration.GetValue<bool>("EFCORE_AUTOMIGRATE", false);

if (app.Environment.IsDevelopment() || autoMigrateEnabled)
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>() as DbContext;

            if (dbContext == null)
            {
                app.Logger.LogCritical("CRÍTICO: IApplicationDbContext no pudo ser resuelto como DbContext para migraciones.");
                throw new InvalidOperationException("No se pudo obtener el DbContext para aplicar migraciones.");
            }

            dbContext.Database.Migrate();
            app.Logger.LogInformation("✔️ Migraciones de EF Core aplicadas correctamente.");
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "❌ ERROR FATAL: Fallo al aplicar las migraciones de EF Core.");
            // En cualquier entorno, la aplicación NO debe continuar si las migraciones fallan.
            throw; // Provoca que el proceso de la API termine inmediatamente.
        }
    }
}
else
{
    app.Logger.LogInformation("ℹ️ Migraciones automáticas de EF Core deshabilitadas. Se espera migración externa (CI/CD).");
}
// --- FIN: Gestión de Migraciones de EF Core ---


// ✅ Pre-carga de taxonomía PGC con la nueva arquitectura
// Este paso se ejecuta AHORA después de que las migraciones aseguren que la BD está lista.
using (var scope = app.Services.CreateScope())
{
    var pgcService = scope.ServiceProvider.GetRequiredService<IPgcTaxonomyService>();
    await pgcService.RunAsync();
}

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Conta360 API v1"));
}

app.UseHttpsRedirection();
app.MapControllers();

// Endpoints mínimos (usando MediatR)
app.MapPost("/api/accounts", async (CreateAccountRequest request, IMediator mediator) =>
{
    var command = new CreateAccountCommand { Name = request.Name, InitialBalance = request.InitialBalance };
    var result = await mediator.Send(command);
    return result.IsSuccess
        ? Results.Created($"/api/accounts/{result.Value}", result.Value)
        : Results.BadRequest(result.Error);
})
.WithName("CreateAccount")
.Produces<Guid>(StatusCodes.Status201Created)
.ProducesProblem(StatusCodes.Status400BadRequest);

app.MapGet("/api/accounts/{id}", async (Guid id, IMediator mediator) =>
{
    var query = new GetAccountByIdQuery { Id = id };
    var result = await mediator.Send(query);
    return result.IsSuccess
        ? Results.Ok(result.Value)
        : Results.NotFound(result.Error);
})
.WithName("GetAccountById")
.Produces<Conta360.Application.DTOs.AccountDto>(StatusCodes.Status200OK)
.ProducesProblem(StatusCodes.Status404NotFound);

app.MapGet("/health", () => Results.Ok("Healthy"));

app.Run();