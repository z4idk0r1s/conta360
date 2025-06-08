using Microsoft.EntityFrameworkCore;

namespace Conta360.Infrastructure.Sqlite.Services;

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
