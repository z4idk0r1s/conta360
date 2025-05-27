using FluentValidation.Results;
using Conta360.Shared.Models.DTOs;
using Conta360.Shared.Models.Validation;
using Conta360.Domain.Interfaces;

namespace Conta360.Application.Interfaces
{
    public interface IConta360Service
    {
        Task<ValidationResult> ValidateInvoices(IEnumerable<EmittedInvoiceDto> emittedInvoices,
                                            IEnumerable<ReceivedInvoiceDto> receivedInvoices);
        Task<ValidationResult> ValidateAccountingEntries(IEnumerable<AccountingEntryDto> entries);
        Task<bool> IsValidPeriod(int year, int month);
    }
}
