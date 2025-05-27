using FluentValidation.Results;
using Conta360.Shared.Models.DTOs;
using Conta360.Shared.Models.Validation.Models;


namespace Conta360.Application.Interfaces
{
    public interface IValidationEngine
    {
        Task<ValidationResult> ValidateEmittedInvoiceAsync(EmittedInvoiceDto invoice);
        Task<ValidationResult> ValidateReceivedInvoiceAsync(ReceivedInvoiceDto invoice);
        Task<ValidationResult> ValidateAccountingEntryAsync(AccountingEntryDto entry);
        Task<IEnumerable<ValidationResult>> ValidateAllAsync(ValidationContext context);
    }
}
