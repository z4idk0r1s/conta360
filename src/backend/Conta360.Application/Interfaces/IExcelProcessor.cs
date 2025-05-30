using Conta360.Shared.Models.DTOs;


namespace Conta360.Application.Interfaces
{
    public interface IExcelProcessor
    {
        Task<(IEnumerable<EmittedInvoiceDto>, IEnumerable<ReceivedInvoiceDto>)> ProcessExcelFile(Stream fileStream);
    }
}

