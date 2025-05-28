using Conta360.Shared.Models.DTOs;


namespace Conta360.Shared.Models.Interfaces
{
    public interface IValidationEngine
    {
        Task<ValidationResult> ValidateEmittedInvoiceAsync(EmittedInvoiceDto invoice);
        Task<ValidationResult> ValidateReceivedInvoiceAsync(ReceivedInvoiceDto invoice);
        Task<ValidationResult> ValidateAccountingEntryAsync(AccountingEntryDto entry);
        Task<IEnumerable<ValidationResult>> ValidateAllAsync(ValidationContext context);
    }
}
