/*using Conta360.Application.Interfaces;
using Conta360.Infrastructure.Postgres.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Conta360.Infrastructure.Postgres.Repositories;
using Conta360.Domain.Interfaces;

namespace Conta360.Infrastructure.Postgres.Services
{
    public static class ServiceRegistrationPostgres
    {
        public static IServiceCollection AddPostgresInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PostgresDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"),
                    b => b.MigrationsAssembly(typeof(PostgresDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext, PostgresDbContext>();
            services.AddScoped<IPgcAccountRepository, AccountRepositoryPostgres>();
            services.AddScoped<IUnitOfWork, UnitOfWorkPostgres>();
            return services;
        }
    }
}
*/