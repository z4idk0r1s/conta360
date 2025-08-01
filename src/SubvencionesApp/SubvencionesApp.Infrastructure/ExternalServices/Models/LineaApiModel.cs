using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class LineaApiModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("codigo")]
        public string? Codigo { get; set; }

        [JsonProperty("nombre")]
        public string? Nombre { get; set; }
    }
}