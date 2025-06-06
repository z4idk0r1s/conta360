namespace Conta360.Infrastructure.PGC.Domain.Models
{
    public class PGCEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Level { get; set; }
        // Other properties related to PGC structure
    }
}