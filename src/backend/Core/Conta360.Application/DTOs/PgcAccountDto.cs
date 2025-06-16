namespace Conta360.Application.DTOs
{
    public class PgcAccountDto
    {
        public string Code { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? Balance { get; set; }
        public string? Namespace { get; set; }
        public string? ConceptId { get; set; }
        public bool IsAbstract { get; set; }
    }
}
