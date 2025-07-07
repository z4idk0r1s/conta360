using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// Usings del proyecto
using Conta360.Application.Behaviours;
using Conta360.Application.Interfaces;
using Conta360.Application.Mappings;
using Conta360.Core.Common;
using Conta360.Core.Interfaces;
using Conta360.Domain.Interfaces;

// Infraestructura - Contextos y repositorios
using Conta360.Infrastructure.Excel.Configuration;
using Conta360.Infrastructure.Excel.Services.Implementation;
using Conta360.Infrastructure.Excel.Services.Interfaces;

using Conta360.Infrastructure.PGC.Processing;
using Conta360.Infrastructure.PGC.Extraction;
using Conta360.Infrastructure.PGC.Services;

using Conta360.Infrastructure.Postgres.Contexts;
using Conta360.Infrastructure.Postgres.Repositories;

using Conta360.Infrastructure.Sqlite.Contexts;
using Conta360.Infrastructure.Sqlite.Repositories;
using Conta360.Infrastructure.Postgres;
using Conta360.Infrastructure.Excel.Services;

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

        public static IServiceCollection AddConta360Infrastructure(this IServiceCollection services, IConfiguration configuration, string dbProvider)
        {
            // === Configuraciones ===
            services.Configure<PgcExtractorOptions>(configuration.GetSection("Pgc"));
            services.Configure<ExcelSettings>(configuration.GetSection("ExcelSettings"));

            // === Excel Services ===
            services.AddScoped<IExcelProcessor, ExcelProcessor>();
            services.AddScoped<IExcelFiscalProcessor, ExcelFiscalProcessor>();

            // === PGC Services ===
            services.AddHttpClient<IPgcTaxonomyDownloader, PgcTaxonomyDownloader>();
            services.AddScoped<PgcTaxonomyValidator>();
            services.AddScoped<PgcTaxonomyParser>();
            services.AddScoped<PgcTaxonomyBuilder>();
            services.AddScoped<IPgcTaxonomyService, PgcTaxonomyService>();

            // === DB Provider Switch ===
            if (dbProvider?.Equals("postgres", StringComparison.OrdinalIgnoreCase) == true)
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

            return services;
        }
    }
}
