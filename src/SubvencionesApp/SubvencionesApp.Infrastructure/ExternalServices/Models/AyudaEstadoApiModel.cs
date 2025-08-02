namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class AyudaEstadoApiModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string InstrumentoId { get; set; }
        public string TipoBeneficiarioId { get; set; }
        public string Estado { get; set; }
    }
}