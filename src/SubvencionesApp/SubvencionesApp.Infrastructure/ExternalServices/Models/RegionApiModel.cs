using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class RegionApiModel
    {
        [JsonProperty("id")]
        public required string Id { get; set; }

        [JsonProperty("nombre")]
        public required string Nombre { get; set; }
    }
}