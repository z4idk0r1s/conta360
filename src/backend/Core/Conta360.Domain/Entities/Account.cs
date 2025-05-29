namespace Conta360.Domain.Entities
{
    public class Account : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }

        // Add other properties as needed
    }

    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}