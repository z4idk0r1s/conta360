using System.Collections.Generic;
using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class ConvocatoriaDetalleApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public required string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public required string Descripcion { get; set; }

        [JsonProperty("detalles")]
        public required string Detalles { get; set; }

        [JsonProperty("estado")]
        public required string Estado { get; set; }

        [JsonProperty("fechaInicio")]
        public required string FechaInicio { get; set; }

        [JsonProperty("fechaFin")]
        public required string FechaFin { get; set; }

        [JsonProperty("fechaPublicacion")]
        public required string FechaPublicacion { get; set; }

        [JsonProperty("organismoId")]
        public required string OrganismoId { get; set; }

        [JsonProperty("regionId")]
        public required string RegionId { get; set; }

        [JsonProperty("tipoBeneficiarioId")]
        public required string TipoBeneficiarioId { get; set; }

        [JsonProperty("instrumentoId")]
        public required string InstrumentoId { get; set; }

        [JsonProperty("plazos")]
        public required List<PlazoApiModel> Plazos { get; set; }

        [JsonProperty("documentos")]
        public required object Documentos { get; set; }

        [JsonProperty("beneficiarios")]
        public required List<BeneficiarioApiModel> Beneficiarios { get; set; }
    }
}