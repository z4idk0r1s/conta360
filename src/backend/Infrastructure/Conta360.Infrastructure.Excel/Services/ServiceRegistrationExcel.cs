/*using Conta360.Core.Common;
using Conta360.Application.Interfaces;
using Conta360.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Conta360.Infrastructure.Excel.Services.Interfaces;
using Conta360.Infrastructure.Excel.Services.Implementation;
using Conta360.Infrastructure.Excel.Configuration;

namespace Conta360.Infrastructure.Excel.Services
{
    public static class ServiceRegistrationExcel
    {
        /// <summary>
        /// Registra servicios de infraestructura Excel (PGC).
        /// </summary>
        public static IServiceCollection AddExcelInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PgcExtractorOptions>(configuration.GetSection("Pgc"));
            services.AddScoped<IExcelProcessor, ExcelProcessor>();
            return services;
        }

        /// <summary>
        /// Registra servicios de procesamiento fiscal Excel (IVA diario y totales).
        /// </summary>
        public static IServiceCollection AddExcelFiscalServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ExcelSettings>(configuration.GetSection("ExcelSettings"));
            services.AddScoped<IExcelFiscalProcessor, ExcelFiscalProcessor>();
            return services;
        }
    }
}
*/