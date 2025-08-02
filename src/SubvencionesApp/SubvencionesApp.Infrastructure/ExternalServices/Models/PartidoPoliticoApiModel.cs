namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class PartidoPoliticoApiModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Importe { get; set; }
        public string Fecha { get; set; }
        public string OrganismoId { get; set; }
    }
}