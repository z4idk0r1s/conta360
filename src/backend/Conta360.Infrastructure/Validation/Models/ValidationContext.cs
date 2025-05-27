using Conta360.Shared.Models.DTOs;
using System.Collections.Generic;

namespace Conta360.Infrastructure.Validation.Models
{
    public class ValidationContext
    {
        public IEnumerable<EmittedInvoiceDto> ? EmittedInvoices { get; set; }
        public IEnumerable<ReceivedInvoiceDto> ? ReceivedInvoices { get; set; }
        public IEnumerable<AccountingEntryDto> ? AccountingEntries { get; set; }
        public Dictionary<string, object>  ? AdditionalData { get; set; }
    }
}
