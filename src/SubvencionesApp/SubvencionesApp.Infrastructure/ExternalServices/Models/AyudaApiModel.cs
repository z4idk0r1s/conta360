namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class AyudaApiModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string OrganismoId { get; set; }
        public string RegionId { get; set; }
        public string TipoBeneficiarioId { get; set; }
        public string InstrumentoId { get; set; }
    }
}