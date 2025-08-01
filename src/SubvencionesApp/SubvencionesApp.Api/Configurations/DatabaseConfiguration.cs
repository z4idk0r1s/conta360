using Microsoft.EntityFrameworkCore;
using SubvencionesApp.Infrastructure.Database;
using System.Reflection;

namespace SubvencionesApp.Api.Configurations
{
    public static class DatabaseConfiguration
    {
        public static void ConfigureDatabase(WebApplicationBuilder builder)
        {
            var provider = builder.Configuration.GetValue<string>("DatabaseProvider") ?? "SQLITE";
            var connectionString = builder.Configuration.GetConnectionString($"{provider}Connection");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"Connection string '{provider}Connection' not found.");
            }

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                switch (provider.ToUpperInvariant())
                {
                    case "POSTGRESQL":
                        options.UseNpgsql(connectionString, opt =>
                        {
                            opt.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                            opt.EnableRetryOnFailure(3);
                        });
                        break;
                    
                    case "SQLITE":
                    default:
                        options.UseSqlite(connectionString, opt =>
                        {
                            opt.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                        });
                        break;
                }

                if (builder.Environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });
        }

        public static async Task EnsureDatabaseCreated(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            try
            {
                if (app.Environment.IsDevelopment())
                {
                    await context.Database.EnsureCreatedAsync();
                }
                else
                {
                    await context.Database.EnsureCreatedAsync();
                }
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Error durante la inicialización de la base de datos");
                throw;
            }
        }
    }
}