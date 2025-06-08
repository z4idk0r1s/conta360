namespace Conta360.Infrastructure.Postgres.Services;

public static class ServiceRegistrationPostgres
{
    public static IServiceCollection AddPostgresInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PostgresDbContext>(options =>
            options.UsePostgres(configuration.GetConnectionString("PostgresConnection"),
                b => b.MigrationsAssembly(typeof(PostgresDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext, PostgresDbContext>();
        return services;
    }
}
