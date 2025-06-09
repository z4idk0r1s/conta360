using Conta360.Application.Interfaces;
using Conta360.Infrastructure.Sqlite.Contexts;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Conta360.Infrastructure.Sqlite.Services
{
    public static class ServiceRegistrationSqlite
    {
        public static IServiceCollection AddSqliteInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SqliteDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("SqliteConnection"),
                    b => b.MigrationsAssembly(typeof(SqliteDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext, SqliteDbContext>();
            return services;
        }
    }
}
