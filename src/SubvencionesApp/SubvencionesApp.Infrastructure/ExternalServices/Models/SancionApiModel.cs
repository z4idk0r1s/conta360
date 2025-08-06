using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class SancionApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public required string Nombre { get; set; }

        [JsonProperty("motivo")]
        public required string Motivo { get; set; }

        [JsonProperty("sancion")]
        public required string Sancion { get; set; }

        [JsonProperty("estado")]
        public required string Estado { get; set; }
    }
}