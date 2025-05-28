using System.Text;
using Conta360.Application.Interfaces;
using Conta360.Infrastructure.Adapters.ExcelProcessor;
using Conta360.Infrastructure.Adapters.ExcelProcessor.Interfaces;
using Conta360.Infrastructure.Adapters.PGCExtractor;
using Conta360.Services.Api;
using Conta360.Shared.Models.DTOs;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Conta360.Infrastructure.Adapters.PGCExtractor.PGCExtractor.Data.Services;
using Conta360.Infrastructure.Adapters.PGCExtractor.PGCExtractor.Logic.Services;
using Conta360.Infrastructure.Adapters.PGCExtractor.PGCExtractor.Tracker.Services;
using Conta360.Domain.Rules.EmittedInvoice;


var builder = WebApplication.CreateBuilder(args);

// Register encoding provider for Excel
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

// Configure CORS with named policy
builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? Array.Empty<string>();
    if (!allowedOrigins.Any())
    {
        Console.WriteLine("⚠️ Advertencia: No se han configurado orígenes CORS.");
    }

    options.AddPolicy("ValidationAppPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("X-Pagination");
    });
});

// Register AutoMapper with all profiles
builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(MappingProfile).Assembly);

// Register existing services with proper lifetime
builder.Services.AddScoped<IExcelProcessor, ExcelProcessor>();
builder.Services.AddScoped<IConta360Service, LocalConta360Service>();
builder.Services.AddScoped<IValidationEngine, ValidationEngine>();
builder.Services.AddScoped<IValidationRule<EmittedInvoiceDto>, TotalAmountValidationRule>();

// ──────────────────────────────────────────────────────────────────────────────
// PGCExtractor integration
// ──────────────────────────────────────────────────────────────────────────────
builder.Services.AddSingleton<PgcTaxonomyBuilder>();
builder.Services.AddSingleton<PgcBoeXmlScraper>();
builder.Services.AddSingleton<AccountClassifier>();
builder.Services.AddSingleton<PgcChangeTracker>();
builder.Services.AddSingleton<XmlValidator>();

var app = builder.Build();

// On application start, download, consolidate and track PGC changes
app.Lifetime.ApplicationStarted.Register(async () =>
{
    var zipUrl   = "https://www.icac.gob.es/sites/default/files/pgc2007/v170/taxonomiaPGC2007_v1.7.0_20240101_r1-EIRL.zip";
    var localZip = Path.Combine("Data", "taxonomia.zip");
    var xmlOut   = Path.Combine("Data", "pgc2007.xml");

    var builderSvc = app.Services.GetRequiredService<PgcTaxonomyBuilder>();
    await builderSvc.BuildConsolidatedXmlAsync(zipUrl, localZip, xmlOut);

    // VALIDACIÓN XML VS XSDs
    var validator = app.Services.GetRequiredService<XmlValidator>();
    var xsdDir    = Path.Combine("Data", "Xsd");
    var xsdFiles  = Directory.Exists(xsdDir)
        ? Directory.GetFiles(xsdDir, "*.xsd").ToList()
        : new List<string>();
    if (xsdFiles.Count > 0)
    {
        validator.ValidateXmlWithMultipleSchemas(xmlOut, xsdFiles);
    }
    else
    {
        Console.WriteLine("⚠️ No se encontraron archivos XSD para validar.");
    }

    var tracker = app.Services.GetRequiredService<PgcChangeTracker>();
    var changed = tracker.CheckForChanges(xmlOut);

    Console.WriteLine(changed
        ? "✅ PGC actualizado y cuentas regeneradas."
        : "ℹ️ Sin cambios detectados en el PGC.");
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else 
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseRouting();
app.UseCors("ValidationAppPolicy");

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
