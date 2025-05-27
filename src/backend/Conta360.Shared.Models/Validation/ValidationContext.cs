using Conta360.Shared.Models.DTOs;
using System.Collections.Generic;
using Conta360.Shared.Models.Validation;


namespace Conta360.Shared.Models.Validation
{
    public class ValidationContext
    {
        public IEnumerable<EmittedInvoiceDto> ? EmittedInvoices { get; set; }
        public IEnumerable<ReceivedInvoiceDto> ? ReceivedInvoices { get; set; }
        public IEnumerable<AccountingEntryDto> ? AccountingEntries { get; set; }
        public Dictionary<string, object>  ? AdditionalData { get; set; }
    }
}
