namespace Conta360.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public Guid AccountId { get; set; }
        public Account? Account { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}