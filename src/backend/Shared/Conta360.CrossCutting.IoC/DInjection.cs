using Conta360.Application.Behaviours;
using Conta360.Application.Interfaces;
using Conta360.Application.Mappings;
using Conta360.Core.Common;
using Conta360.Core.Interfaces;
using Conta360.Domain.Interfaces;
using Conta360.Infrastructure.Excel.Services;
using Conta360.Infrastructure.Postgres;
using Conta360.Infrastructure.Postgres.Contexts;
using Conta360.Infrastructure.Sqlite.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Collections.Generic;

namespace Conta360.CrossCutting.IoC
{
    public static class DInjection
    {
        public static IServiceCollection AddConta360Application(this IServiceCollection services)
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
            // Configuración global
            services.Configure<PgcExtractorOptions>(configuration.GetSection("Pgc"));

            // Base repositorios
            services.AddScoped<IPgcAccountRepository, IPgcAccountRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Infraestructuras específicas (uso de métodos modulares)
            services.AddExcelFiscalServices(configuration);
            services.AddExcelInfrastructure(configuration);

            // Base de datos
            if (dbProvider == "Postgres")
            {
                services.AddDbContext<PostgresDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"),
                        b => b.MigrationsAssembly(typeof(PostgresDbContext).Assembly.FullName)));

                services.AddScoped<IApplicationDbContext, PostgresDbContext>();
            }
            else
            {
                services.AddDbContext<SqliteDbContext>(options =>
                    options.UseSqlite(configuration.GetConnectionString("SqliteConnection"),
                        b => b.MigrationsAssembly(typeof(SqliteDbContext).Assembly.FullName)));

                services.AddScoped<IApplicationDbContext, SqliteDbContext>();
            }

            return services;
        }
    }
}
