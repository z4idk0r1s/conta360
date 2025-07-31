using SubvencionesApp.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// CONFIGURACIÓN DE LA BASE DE DATOS
// ========================================
DatabaseConfiguration.ConfigureDatabase(builder);

// ========================================
// CONFIGURACIÓN DE SERVICIOS
// ========================================
ApplicationServicesConfiguration.ConfigureApplicationServices(builder);
InfrastructureConfiguration.ConfigureInfrastructureServices(builder);
DomainConfiguration.ConfigureDomainServices(builder);

// ========================================
// CONFIGURACIÓN DE CONTROLADORES Y API
// ========================================
ApiConfiguration.ConfigureApiServices(builder);

var app = builder.Build();

// ========================================
// PIPELINE DE LA APLICACIÓN
// ========================================
PipelineConfiguration.ConfigurePipeline(app);

// ========================================
// INICIALIZACIÓN DE LA BASE DE DATOS
// ========================================
await DatabaseConfiguration.EnsureDatabaseCreated(app);

app.Run();