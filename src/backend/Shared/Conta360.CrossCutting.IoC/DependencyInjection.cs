using Conta360.Application.Behaviours;
using Conta360.Application.Interfaces;
using Conta360.Application.Mappings;
using Conta360.Infrastructure.Excel.Services;
using Conta360.Infrastructure.PGC.Services;
using Conta360.Infrastructure.Postgres.Contexts;
using Conta360.Infrastructure.Postgres.Repositories;
using Conta360.Infrastructure.Reporting.Services;
using Conta360.Infrastructure.Postgres; // For UnitOfWork
using Conta360.Persistence.Contexts;
using Conta360.Persistence.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Conta360.Core.Interfaces; // For IDateTimeProvider, ICurrentUserService
using Conta360.Core.Common; // For Guard, Error, OperationResult (though not services for DI)

namespace Conta360.CrossCutting.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddConta360Application(this IServiceCollection services)
        {
            // MediatR Configuration
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); // This assembly (IoC)
                cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly); // Application assembly

                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            // AutoMapper Configuration
            services.AddAutoMapper(Assembly.GetExecutingAssembly()); // This assembly
            services.AddAutoMapper(typeof(MappingProfile).Assembly); // Application assembly

            // FluentValidation Configuration
            services.AddValidatorsFromAssembly(typeof(MappingProfile).Assembly); // Scans Application assembly for validators

            return services;
        }

        public static IServiceCollection AddConta360Infrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Persistence
            services.AddScoped<IApplicationDbContext, PostgresDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(AccountRepository)); // Generic repository, placeholder for all
                                                                               // In a real app, you would have specific implementations for each IRepository<T> if needed

            // Add EF Core DbContext for Postgres
            services.AddDbContext<PostgresDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"),
                    b => b.MigrationsAssembly(typeof(PostgresDbContext).Assembly.FullName)));

            // Add EF Core DbContext for Sqlite (if used for testing/local, conditional setup might be better)
            services.AddDbContext<Conta360.Infrastructure.Sqlite.Contexts.SqliteDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("SqliteConnection"),
                    b => b.MigrationsAssembly(typeof(Conta360.Infrastructure.Sqlite.Contexts.SqliteDbContext).Assembly.FullName)));


            // Infrastructure Services
            services.AddScoped<IExcelProcessor, ExcelProcessor>();
            services.AddScoped<IPGCStructureService, PGCStructureService>();
            services.AddScoped<IFinancialReportingService, FinancialReportingService>();
            services.AddScoped<IKpiCalculationService, KpiCalculationService>();

            // Internal PGC Extractor Services
            services.AddScoped<PGCExtractor.Data.Services.PGCDataExtractor>();
            services.AddScoped<PGCExtractor.Logic.Services.PGCProcessor>();

            // Core Services (if they have concrete implementations and are not just interfaces/statics)
            // For example, if ICurrentUserService has a concrete implementation:
            // services.AddScoped<ICurrentUserService, CurrentUserService>(); // Assuming CurrentUserService is in API or Infrastructure

            return services;
        }
    }
}