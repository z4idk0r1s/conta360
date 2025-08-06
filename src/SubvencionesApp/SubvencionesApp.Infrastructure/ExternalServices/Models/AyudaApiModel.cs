using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class AyudaApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public required string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public required string Descripcion { get; set; }

        [JsonProperty("organismoId")]
        public required string OrganismoId { get; set; }

        [JsonProperty("regionId")]
        public required string RegionId { get; set; }

        [JsonProperty("tipoBeneficiarioId")]
        public required string TipoBeneficiarioId { get; set; }

        [JsonProperty("instrumentoId")]
        public required string InstrumentoId { get; set; }
    }
}