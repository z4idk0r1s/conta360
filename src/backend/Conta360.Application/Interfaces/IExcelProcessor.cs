using Conta360.Shared.Models.DTOs;
using Conta360.Shared.Models.Validation;


namespace Conta360.Application.Interfaces
{
    public interface IExcelProcessor
    {
        Task<(IEnumerable<EmittedInvoiceDto>, IEnumerable<ReceivedInvoiceDto>)> ProcessExcelFile(Stream fileStream);
    }
}

