namespace Conta360.Application.DTOs
{
    public class PgcAccountTreeDto
    {
        public string Code { get; set; } = default!;
        public string Description { get; set; } = default!;
        public List<PgcAccountTreeDto> Children { get; set; } = new();
    }
}
