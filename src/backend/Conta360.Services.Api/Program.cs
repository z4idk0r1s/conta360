using System.Text;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

using AutoMapper;
using AutoMapper.Collections;

using Conta360.Application.Interfaces;
using Conta360.Domain.Rules.EmittedInvoice;
using Conta360.Infrastructure.Adapters.ExcelProcessor;
using Conta360.Infrastructure.Adapters.ExcelProcessor.Interfaces;
using Conta360.Infrastructure.Adapters.PGCExtractor;
using Conta360.Infrastructure.Adapters.PGCExtractor.PGCExtractor.Data.Services;
using Conta360.Infrastructure.Adapters.PGCExtractor.PGCExtractor.Logic.Services;
using Conta360.Infrastructure.Adapters.PGCExtractor.PGCExtractor.Tracker.Services;
using Conta360.Services.Api;
using Conta360.Shared.Models.DTOs;
using Conta360.Shared.Models.Interfaces;
using Conta360.Shared.Models.Validation;

var builder = WebApplication.CreateBuilder(args);

// Register encoding provider for legacy code pages (Excel)
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// ──────────────────────────────────────────────────────────────────────────────
// Configure Services (Dependency Injection)
// ──────────────────────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

// AutoMapper setup
builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(MappingProfile).Assembly);

// CORS Configuration
builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? Array.Empty<string>();
    
    if (!allowedOrigins.Any())
        Console.WriteLine("⚠️ Advertencia: No se han configurado orígenes CORS.");

    options.AddPolicy("ValidationAppPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("X-Pagination");
    });
});

// Domain & Application Services
builder.Services.AddScoped<IExcelProcessor, ExcelProcessor>();
builder.Services.AddScoped<IConta360Service, LocalConta360Service>();
builder.Services.AddScoped<IValidationEngine, ValidationEngine>();
builder.Services.AddScoped<IValidationRule<EmittedInvoiceDto>, TotalAmountValidationRule>();

// PGCExtractor Services
builder.Services.AddSingleton<PgcTaxonomyBuilder>();
builder.Services.AddSingleton<PgcBoeXmlScraper>();
builder.Services.AddSingleton<AccountClassifier>();
builder.Services.AddSingleton<PgcChangeTracker>();
builder.Services.AddSingleton<XmlValidator>();

var app = builder.Build();

// ──────────────────────────────────────────────────────────────────────────────
// Startup Logic
// ──────────────────────────────────────────────────────────────────────────────
app.Lifetime.ApplicationStarted.Register(async () =>
{
    const string zipUrl = "https://www.icac.gob.es/sites/default/files/pgc2007/v170/taxonomiaPGC2007_v1.7.0_20240101_r1-EIRL.zip";
    var dataDir = "Data";
    var localZip = Path.Combine(dataDir, "taxonomia.zip");
    var xmlOut = Path.Combine(dataDir, "pgc2007.xml");

    var builderSvc = app.Services.GetRequiredService<PgcTaxonomyBuilder>();
    await builderSvc.BuildConsolidatedXmlAsync(zipUrl, localZip, xmlOut);

    var xsdDir = Path.Combine(dataDir, "Xsd");
    var xsdFiles = Directory.Exists(xsdDir) ? Directory.GetFiles(xsdDir, "*.xsd").ToList() : new();

    if (xsdFiles.Any())
    {
        var validator = app.Services.GetRequiredService<XmlValidator>();
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

// ──────────────────────────────────────────────────────────────────────────────
// Middleware Pipeline
// ──────────────────────────────────────────────────────────────────────────────
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
