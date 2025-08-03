using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class AyudaEstadoApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public required string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public required string Descripcion { get; set; }

        [JsonProperty("instrumentoId")]
        public required string InstrumentoId { get; set; }

        [JsonProperty("tipoBeneficiarioId")]
        public required string TipoBeneficiarioId { get; set; }

        [JsonProperty("estado")]
        public required string Estado { get; set; }
    }
}