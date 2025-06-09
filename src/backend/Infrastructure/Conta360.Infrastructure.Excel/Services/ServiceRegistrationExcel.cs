using Conta360.Core.Common;
using Conta360.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Conta360.Infrastructure.Excel.Services
{
    public static class ServiceRegistrationExcel
    {
        public static IServiceCollection AddExcelInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<PgcExtractorOptions>(config.GetSection("Pgc"));
            services.AddScoped<IExcelProcessor, ExcelProcessor>();
            return services;
        }
    }
}
