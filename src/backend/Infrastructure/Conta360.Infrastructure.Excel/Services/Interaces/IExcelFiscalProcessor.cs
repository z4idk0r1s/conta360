using System.Threading;
using System.Threading.Tasks;
using Conta360.Infrastructure.Excel.Models.ResumenFiscal;

namespace Conta360.Infrastructure.Excel.Services.Interfaces
{
    public interface IExcelFiscalProcessor
    {
        Task<ResumenFiscalResponse> ProcesarResumenFiscalAsync(
            CancellationToken cancellationToken = default);
    }
}