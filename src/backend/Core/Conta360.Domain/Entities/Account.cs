namespace Conta360.Domain.Entities
{
    public class Account : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public IList<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}