using Conta360.Application.Features.Accounts.Commands.CreateAccount;
using Conta360.Application.Features.Accounts.Commands.CreateAccount.Queries;
using Conta360.CrossCutting.IoC;
using MediatR;
using Microsoft.OpenApi.Models;
using Serilog;
using Conta360.Presentation.Api.Models;
using Conta360.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Logging profesional con Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day));

// Servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Conta360 API", Version = "v1" });
});

// Registro de dependencias
builder.Services
    .AddConta360Application(builder.Configuration)
    .AddConta360Infrastructure(builder.Configuration, dbProvider: "Sqlite"); // o "Postgres"

var app = builder.Build();

// ✅ Pre-carga de taxonomía PGC con la nueva arquitectura
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
