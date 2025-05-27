using System;

namespace Conta360.Shared.Models.DTOs
{
    public class AccountingEntryDto
    {
        public string Id { get; set; }
        public string EntryId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string AccountCode { get; set; }
        
    }
}
