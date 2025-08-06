using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class TerceroApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public required string Nombre { get; set; }

        [JsonProperty("nif")]
        public required string Nif { get; set; }

        [JsonProperty("tipo")]
        public required string Tipo { get; set; }
    }
}