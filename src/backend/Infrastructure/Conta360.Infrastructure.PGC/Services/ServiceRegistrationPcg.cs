using Conta360.Core.Interfaces;
using Conta360.Application.Interfaces;
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
            services.AddHttpClient<IPgcTaxonomyDownloader, PgcTaxonomyDownloader>();
            services.AddScoped<IPgcTaxonomyValidator, PgcTaxonomyValidator>();
            services.AddScoped<PgcTaxonomyBuilder>();
            services.AddScoped<IPgcTaxonomyService, PgcTaxonomyService>();
            return services;
        }
    }
}
