using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class AyudaApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("organismoId")]
        public string OrganismoId { get; set; }

        [JsonProperty("regionId")]
        public string RegionId { get; set; }

        [JsonProperty("tipoBeneficiarioId")]
        public string TipoBeneficiarioId { get; set; }

        [JsonProperty("instrumentoId")]
        public string InstrumentoId { get; set; }
    }
}