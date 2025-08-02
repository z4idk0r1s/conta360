using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class AyudaEstadoApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("instrumentoId")]
        public string InstrumentoId { get; set; }

        [JsonProperty("tipoBeneficiarioId")]
        public string TipoBeneficiarioId { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; }
    }
}