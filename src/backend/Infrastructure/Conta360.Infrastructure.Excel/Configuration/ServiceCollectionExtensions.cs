using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Conta360.Infrastructure.Excel.Services.Interfaces;
using Conta360.Infrastructure.Excel.Services.Implementation;

namespace Conta360.Infrastructure.Excel.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddExcelServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<ExcelSettings>(
                configuration.GetSection("ExcelSettings"));
            
            services.AddScoped<IExcelFiscalProcessor, ExcelFiscalProcessor>();
            
            return services;
        }
    }
}