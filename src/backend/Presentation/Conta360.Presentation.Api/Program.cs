using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Serilog;
using Conta360.Application.Features.Accounts.Commands.CreateAccount;
using Conta360.Application.Features.Accounts.Queries.GetAccountById;
using Conta360.Core.Common;
using Conta360.Core.Interfaces;
using Conta360.Application.Interfaces;
using Conta360.Infrastructure.PGC.Processing;
using Conta360.Presentation.Api.Models;
using Conta360.CrossCutting.IoC;


var builder = WebApplication.CreateBuilder(args);

// 0) Configurar PgcExtractorOptions desde appsettings.json
builder.Services.Configure<PgcExtractorOptions>(builder.Configuration.GetSection("Pgc"));

// 1) Configurar Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));

// 2) Añadir servicios para controladores, Swagger y endpoints
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3) Registrar Application + Infrastructure
builder.Services
    .AddConta360Application()
    .AddSqliteInfrastructure(builder.Configuration)
    .AddExcelInfrastructure()
    .AddPGCInfrastructure(builder.Configuration);

// 4) Configurar JWT Authentication (en este momento no hay nada implementado)

var app = builder.Build();

// 5) En el arranque, procesar la taxonomía PGC (si EnableStartupDownload == true)
using (var scope = app.Services.CreateScope())
{
    var pgcProcessor = scope.ServiceProvider.GetRequiredService<IPgcProcessor>();
    await pgcProcessor.SystemProcessAsync(CancellationToken.None);
}

// 6) Configurar pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers(); // Para controladores tradicionales (si hay)


// 7) Minimal API Endpoints para Cuentas (opcional, puedes moverlos a un Controller si prefieres)
app.MapPost("/api/accounts", async (CreateAccountRequest request, IMediator mediator) =>
{
    var command = new CreateAccountCommand 
    { 
        Name = request.Name, 
        InitialBalance = request.InitialBalance 
    };
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


app.Run();
