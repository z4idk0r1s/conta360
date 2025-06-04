using System;

namespace Conta360.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public decimal Amount { get; set; }
        public int Account { get; set; }
        public DateTime Date { get; set; }
        public int AccountId { get; set; }
    }
}
