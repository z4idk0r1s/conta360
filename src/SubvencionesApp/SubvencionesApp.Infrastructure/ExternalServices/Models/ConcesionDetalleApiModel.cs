namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class ConcesionDetalleApiModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Detalles { get; set; }
        public decimal Importe { get; set; }
        public string BeneficiarioId { get; set; }
        public string OrganismoId { get; set; }
        public string FechaResolucion { get; set; }
    }
}