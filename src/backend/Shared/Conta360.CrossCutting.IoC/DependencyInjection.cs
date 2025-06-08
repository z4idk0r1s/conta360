using Conta360.Core.Interfaces;
using Conta360.Domain.Interfaces;
using Conta360.Application.Behaviours;
using Conta360.Application.Interfaces;
using Conta360.Application.Mappings;
using Conta360.Infrastructure.Excel.Services;
using Conta360.Infrastructure.Sqlite.Contexts;
using Conta360.Infrastructure.Reporting.Services;
using Conta360.Infrastructure.PGC.Processing;
using Conta360.Infrastructure.Postgres;
using Conta360.Infrastructure.Postgres.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Conta360.CrossCutting.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConta360Application(this IServiceCollection services)
        {
            // 1) MediatR Configuration
            services.AddMediatR(cfg =>
            {
                // Registra handlers y comportamientos en el ensamblado de Application
                cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly);
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            // 2) AutoMapper Configuration
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // 3) FluentValidation Configuration
            services.AddValidatorsFromAssembly(typeof(MappingProfile).Assembly);

            return services;
        }

        public static IServiceCollection AddConta360Infrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 1) Configurar opciones de PGC (IOptions<PgcExtractorOptions>)
            services.Configure<PgcExtractorOptions>(configuration.GetSection("Pgc"));

            // 2) Registrar DbContext SQLite como IApplicationDbContext
            //    (deja esto preparado para cambiar fácilmente a Postgres en el futuro)
            services.AddDbContext<SqliteDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("SqliteConnection"),
                    b => b.MigrationsAssembly(typeof(SqliteDbContext).Assembly.FullName)));
            services.AddScoped<IApplicationDbContext, SqliteDbContext>();

            //    --- Si en el futuro se desea cambiar a Postgres,
            //    reemplazar lo anterior por:
            //    services.AddDbContext<PostgresDbContext>(options =>
            //        options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"),
            //            b => b.MigrationsAssembly(typeof(PostgresDbContext).Assembly.FullName)));
            //    services.AddScoped<IApplicationDbContext, PostgresDbContext>();

            // 3) Repositorios y UnitOfWork (Postgres o SQLite, según contexto)
            //    (suponiendo que IAccountRepository y UnitOfWork estén adaptados a IApplicationDbContext)
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //    En caso de tener repositorios genéricos:
            //    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            // 4) Servicios de Infraestructura (no relacionados con PGC)
            services.AddScoped<IExcelProcessor, ExcelProcessor>();
            services.AddScoped<IKpiCalculationService, KpiCalculationService>();

            // 5) PGC Extractor: downloader + processor
            //    * IPgcTaxonomyDownloader  → PgcTaxonomyDownloader (HttpClient configurado)
            services.AddHttpClient<IPgcTaxonomyDownloader, PgcTaxonomyDownloader>(client =>
            {
                client.Timeout = TimeSpan.FromMinutes(2);
            });

            //    * IPgcProcessor → PgcProcessor
            services.AddScoped<IPgcProcessor, PgcProcessor>();

            return services;
        }
    }
}
