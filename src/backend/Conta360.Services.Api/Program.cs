// System
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

// ASP.NET Core
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Entity Framework Core / ORM
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dapper;

// Validación
using FluentValidation;
using FluentValidation.Results;

// Utilidades / 3rd Party
using AutoMapper;
using MediatR;
using Serilog;

// Swagger
using Swashbuckle.AspNetCore.Annotations;

// Conta360.Application
using Conta360.Application;
using Conta360.Application.DTOs;
using Conta360.Application.Interfaces;
using Conta360.Application.UseCases;

// Conta360.Domain
using Conta360.Domain;
using Conta360.Domain.Entities;
using Conta360.Domain.ValueObjects;
using Conta360.Domain.Rules;
using Conta360.Domain.Rules.EmittedInvoice;
using Conta360.Domain.Mapping;

// Conta360.Infrastructure
using Conta360.Infrastructure;
using Conta360.Infrastructure.Adapters;
using Conta360.Infrastructure.Adapters.ExcelProcessor;
using Conta360.Infrastructure.Adapters.PGCExtractor;
using Conta360.Infrastructure.Adapters.PGCExtractor.Core;
using Conta360.Infrastructure.Adapters.PGCExtractor.Core.Models;
using Conta360.Infrastructure.Adapters.PGCExtractor.Data;
using Conta360.Infrastructure.Adapters.PGCExtractor.Data.Services;
using Conta360.Infrastructure.Adapters.PGCExtractor.Logic;
using Conta360.Infrastructure.Adapters.PGCExtractor.Logic.Services;
using Conta360.Infrastructure.Adapters.PGCExtractor.Tracker;
using Conta360.Infrastructure.Adapters.PGCExtractor.Tracker.Services;
using Conta360.Infrastructure.Repositories;
using Conta360.Infrastructure.Validation;
using Conta360.Infrastructure.Validation.Models;

// Conta360.Persistence
using Conta360.Persistence;
using Conta360.Persistence.postgres;
using Conta360.Persistence.sqlite;

// Conta360.SDK
using Conta360.SDK;

// Conta360.Services.Api
using Conta360.Services.Api;
using Conta360.Services.Api.Controllers;

// Conta360.Shared.Models
using Conta360.Shared.Models;
using Conta360.Shared.Models.DTOs;
using Conta360.Shared.Models.Enums;


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
