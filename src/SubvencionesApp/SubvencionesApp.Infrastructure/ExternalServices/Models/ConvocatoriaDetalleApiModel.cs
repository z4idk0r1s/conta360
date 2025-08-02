using System.Collections.Generic;
using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class ConvocatoriaDetalleApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("detalles")]
        public string Detalles { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; }

        [JsonProperty("fechaInicio")]
        public string FechaInicio { get; set; }

        [JsonProperty("fechaFin")]
        public string FechaFin { get; set; }

        [JsonProperty("fechaPublicacion")]
        public string FechaPublicacion { get; set; }

        [JsonProperty("organismoId")]
        public string OrganismoId { get; set; }

        [JsonProperty("regionId")]
        public string RegionId { get; set; }

        [JsonProperty("tipoBeneficiarioId")]
        public string TipoBeneficiarioId { get; set; }

        [JsonProperty("instrumentoId")]
        public string InstrumentoId { get; set; }

        [JsonProperty("plazos")]
        public List<PlazoApiModel> Plazos { get; set; }

        [JsonProperty("documentos")]
        public object Documentos { get; set; }

        [JsonProperty("beneficiarios")]
        public List<BeneficiarioApiModel> Beneficiarios { get; set; }
    }
}