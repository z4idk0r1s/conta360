using System.Collections.Generic;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class ConvocatoriaDetalleApiModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Detalles { get; set; }
        public string Estado { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string FechaPublicacion { get; set; }
        public string OrganismoId { get; set; }
        public string RegionId { get; set; }
        public string TipoBeneficiarioId { get; set; }
        public string InstrumentoId { get; set; }
        public List<PlazoApiModel> Plazos { get; set; }
        public object Documentos { get; set; }
        public List<BeneficiarioApiModel> Beneficiarios { get; set; }
    }
}