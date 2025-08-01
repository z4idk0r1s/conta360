using Microsoft.OpenApi.Models;
using SubvencionesApp.Infrastructure.Database;
using System.Reflection;

namespace SubvencionesApp.Api.Configurations
{
    public static class ApiConfiguration
    {
        public static void ConfigureApiServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = false;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Subvenciones API", 
                    Version = "v1",
                    Description = "API para la gestión de subvenciones del BDNS",
                    Contact = new OpenApiContact
                    {
                        Name = "Equipo de Desarrollo",
                        Email = "desarrollo@subvenciones.es"
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            builder.Services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            builder.Services.AddMemoryCache();
            
            builder.Services.AddHealthChecks()
                .AddDbContext<AppDbContext>();
        }
    }
}