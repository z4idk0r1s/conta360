using Conta360.Application.Features.Accounts.Commands.CreateAccount;
using Conta360.Application.Features.Accounts.Commands.CreateAccount.Queries;
using Conta360.CrossCutting.IoC;
using MediatR;
using Microsoft.OpenApi.Models;
using Serilog;
using Conta360.Presentation.Api.Models;
using Conta360.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
// orquest
using Conta360.Infrastructure.Reporting.Services;
using System.IO;
using System.Collections.Generic;
////////////////////////////////////////////////

var builder = WebApplication.CreateBuilder(args);
var dbProvider = builder.Configuration.GetValue<string>("Database:Provider") ?? "sqlite";

// Logging profesional con Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));

// --- INICIO: CÓDIGO DE DIAGNÓSTICO DE CONFIGURACIÓN PGC ---
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});
var logger = LoggerFactory.Create(config => config.AddConsole()).CreateLogger<Program>();

var pgcSection = builder.Configuration.GetSection("Pgc");
var extractDirectoryValue = pgcSection["ExtractDirectory"];
var taxonomyZipUrlValue = pgcSection["TaxonomyZipUrl"];

logger.LogInformation("Database Provider leído: {dbProvider}", dbProvider);
logger.LogInformation("DIAGNÓSTICO CONFIGURACIÓN PGC:");
logger.LogInformation("   Sección 'Pgc' existe: {PgcSectionExists}", pgcSection != null);
if (pgcSection != null)
{
    logger.LogInformation("   ExtractDirectory desde IConfiguration: '{ExtractDirectory}'", extractDirectoryValue);
    logger.LogInformation("   TaxonomyZipUrl desde IConfiguration: '{TaxonomyZipUrl}'", taxonomyZipUrlValue);
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
    .AddConta360Infrastructure(builder.Configuration, dbProvider);

var app = builder.Build();

// --- INICIO: Gestión de Migraciones de EF Core (Condicional) ---
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
            throw;
        }
    }
}
else
{
    app.Logger.LogInformation("ℹ️ Migraciones automáticas de EF Core deshabilitadas. Se espera migración externa (CI/CD).");
}
// --- FIN: Gestión de Migraciones de EF Core ---


// ✅ Pre-carga de taxonomía PGC con la nueva arquitectura
using (var scope = app.Services.CreateScope())
{
    var pgcService = scope.ServiceProvider.GetRequiredService<IPgcTaxonomyService>();
    var result = await pgcService.RunAndGetAccountsAsync();

    if (result.IsSuccess)
    {
        app.Logger.LogInformation("✔️ Carga de taxonomía PGC completada con éxito. Se procesaron {Count} cuentas.", result.Value?.Count ?? 0);
    }
    else
    {
        app.Logger.LogError("❌ ERROR: Fallo en la carga de la taxonomía PGC: {ErrorMessage}", result.Error?.Description ?? "Error desconocido");
    }
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

// --- Novedad: Endpoint para integrar Excel con A3 ---
app.MapPost("/api/a3/generate-from-excel", async (GenerateA3Request request, ExcelToA3IntegrationService service, ILogger<Program> programLogger) =>
{
    programLogger.LogInformation("API: Solicitud recibida para generar fichero A3 desde Excel. Archivo: '{ExcelPath}'", request.ExcelFilePath);
    try
    {
        string generatedFilePath = await service.GenerateA3FileFromExcelAsync(request.ExcelFilePath, request.A3OutputFilename);
        return Results.Ok(new { Message = "Fichero A3 generado exitosamente.", FilePath = generatedFilePath });
    }
    catch (FileNotFoundException ex)
    {
        programLogger.LogError(ex, "API: Archivo Excel no encontrado: '{ExcelPath}'", request.ExcelFilePath);
        return Results.NotFound(new { Message = $"El archivo Excel especificado no se encontró: {ex.Message}" });
    }
    catch (InvalidOperationException ex)
    {
        programLogger.LogError(ex, "API: Error de operación al generar el fichero A3: {ErrorMessage}", ex.Message);
        return Results.BadRequest(new { Message = $"Error en la operación de generación del fichero A3: {ex.Message}" });
    }
    catch (Exception ex)
    {
        programLogger.LogError(ex, "API: Error inesperado al generar el fichero A3 desde Excel.");
        return Results.Problem(
            statusCode: StatusCodes.Status500InternalServerError,
            title: "Error interno del servidor",
            detail: "Ocurrió un error inesperado al generar el fichero A3.",
            instance: request.ExcelFilePath,
            extensions: new Dictionary<string, object?> { { "details", ex.Message } }
        );
    }
})
.WithName("GenerateA3FromExcel")
.Produces<object>(StatusCodes.Status200OK)
.ProducesProblem(StatusCodes.Status400BadRequest)
.ProducesProblem(StatusCodes.Status404NotFound)
.ProducesProblem(StatusCodes.Status500InternalServerError);

app.Run();
