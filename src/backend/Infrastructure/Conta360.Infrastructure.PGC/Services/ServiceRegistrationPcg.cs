using Conta360.Core.Interfaces;
using Conta360.Core.Common;
using Conta360.Infrastructure.PGC.Processing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace Conta360.Infrastructure.PGC.Services
{
    public static class ServiceRegistrationPgc
    {
        public static IServiceCollection AddPGCInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<PgcExtractorOptions>(config.GetSection("Pgc"));
            services.AddScoped<IPgcProcessor, PgcProcessor>();
            services.AddScoped<IPgcImporter, PgcImporter>();
            services.AddHttpClient<IPgcTaxonomyDownloader, PgcTaxonomyDownloader>();
            return services;
        }
    }
}
