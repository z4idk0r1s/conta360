using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

// Usings del proyecto
using Conta360.Application.Behaviours;
using Conta360.Application.Interfaces;
using Conta360.Application.Mappings;
using Conta360.Application.Services;
using Conta360.Core.Common;
using Conta360.Core.Interfaces;
using Conta360.Domain.Interfaces;
using Conta360.Infrastructure.Excel.Services;
using Conta360.Infrastructure.Postgres;
using Conta360.Infrastructure.Postgres.Contexts;
using Conta360.Infrastructure.Postgres.Repositories;
using Conta360.Infrastructure.Sqlite.Contexts;
using Conta360.Infrastructure.Sqlite.Repositories;
using Conta360.Infrastructure.PGC.Services;
using Conta360.Infrastructure.PGC.Processing;


namespace Conta360.CrossCutting.IoC
{
    public static class DInjection
    {
        public static IServiceCollection AddConta360Application(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly);
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddValidatorsFromAssembly(typeof(MappingProfile).Assembly);

            return services;
        }

        public static IServiceCollection AddConta360Infrastructure(this IServiceCollection services, IConfiguration configuration, string dbProvider = "Sqlite")
        {
            // Configuración global para PGC
            services.Configure<PgcExtractorOptions>(configuration.GetSection("Pgc"));

            // Infraestructura Excel
            services.AddExcelFiscalServices(configuration);
            services.AddExcelInfrastructure(configuration);

            // Infraestructura PGC (descarga, validación, parser, builder, service)
            services.AddPGCInfrastructure(configuration);

            // Base de datos y Unit of Work
            if (dbProvider == "Postgres")
            {
                services.AddPostgresInfrastructure(configuration);
            }
            else // Default a Sqlite
            {
                services.AddSqliteInfrastructure(configuration);
            }

            return services;
        }
    }
}