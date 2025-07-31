using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.Api.Models
{
    public class LineaApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("codigo")]
        public string? Codigo { get; set; }

        [JsonProperty("nombre")]
        public string? Nombre { get; set; }
    }
}