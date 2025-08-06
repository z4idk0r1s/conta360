namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class BeneficiarioApiModel
    {
        public Guid Id { get; set; }
        public required string Nombre { get; set; }
        public required string Identificacion { get; set; }
    }
}