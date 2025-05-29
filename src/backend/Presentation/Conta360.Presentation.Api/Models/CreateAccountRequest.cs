namespace Conta360.Presentation.Api.Models
{
    public class CreateAccountRequest
    {
        public string Name { get; set; } = string.Empty;
        public decimal InitialBalance { get; set; }
    }
}