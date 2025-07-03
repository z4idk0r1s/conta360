using Conta360.Application.Behaviours;
using Conta360.Application.Interfaces;
using Conta360.Application.Mappings;
using Conta360.Core.Common;
using Conta360.Domain.Interfaces;
using Conta360.Infrastructure.Excel.Services;
using Conta360.Infrastructure.PGC.Services;
using Conta360.Infrastructure.Postgres;
using Conta360.Infrastructure.Postgres.Contexts;
using Conta360.Infrastructure.Sqlite.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Conta360.Infrastructure.PGC.Processing;
using Conta360.Infrastructure.Sqlite.Repositories;
using Conta360.Infrastructure.Postgres.Repositories;

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

            // Infraestructura PGC (descarga, validación, builder, service)
            services.AddPGCInfrastructure(configuration);

            // Base de datos y Unit of Work
            if (dbProvider == "Postgres")
            {
                services.AddDbContext<PostgresDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"),
                        b => b.MigrationsAssembly(typeof(PostgresDbContext).Assembly.FullName)));

                services.AddScoped<IApplicationDbContext, PostgresDbContext>();
                services.AddScoped<IPgcAccountRepository, AccountRepositoryPostgres>();
                services.AddScoped<IUnitOfWork, UnitOfWorkPostgres>();
            }
            else
            {
                services.AddDbContext<SqliteDbContext>(options =>
                    options.UseSqlite(configuration.GetConnectionString("SqliteConnection"),
                        b => b.MigrationsAssembly(typeof(SqliteDbContext).Assembly.FullName)));

                services.AddScoped<IApplicationDbContext, SqliteDbContext>();
                services.AddScoped<IPgcAccountRepository, AccountRepositorySqlite>();
                services.AddScoped<IUnitOfWork, UnitOfWorkSqlite>();
            }

            // ...otros registros...
            services.AddScoped<PgcTaxonomyDownloader>(); // Si hace falta (se esta llamando en run init)

            return services;
        }
    }
}
