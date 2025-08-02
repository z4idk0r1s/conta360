namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class GrandeBeneficiarioApiModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public decimal Importe { get; set; }
    }
}