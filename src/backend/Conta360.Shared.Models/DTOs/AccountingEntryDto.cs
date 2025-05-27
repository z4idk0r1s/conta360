using System;

namespace Conta360.Shared.Models.DTOs
{
    public class AccountingEntryDto
    {
        public required string Id { get; set; }
        public required string EntryId { get; set; }
        public required DateTime Date { get; set; }
        public required decimal Amount { get; set; }
        public required string Description { get; set; }
        public required string AccountCode { get; set; }
    }
}