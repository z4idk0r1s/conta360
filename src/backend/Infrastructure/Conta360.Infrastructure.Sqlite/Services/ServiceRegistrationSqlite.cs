using Conta360.Application.Interfaces;
using Conta360.Infrastructure.Sqlite.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Conta360.Infrastructure.Sqlite.Repositories;
using Conta360.Domain.Interfaces;

namespace Conta360.Infrastructure.Sqlite.Services
{
    public static class ServiceRegistrationSqlite
    {
        public static IServiceCollection AddSqliteInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SqliteDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("SqliteConnection"),
                    b => b.MigrationsAssembly(typeof(SqliteDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext, SqliteDbContext>();
            services.AddScoped<IPgcAccountRepository, AccountRepositorySqlite>();
            services.AddScoped<IUnitOfWork, UnitOfWorkSqlite>();

            return services;            
        }
    }
}
