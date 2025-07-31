using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class ProvinciaApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("descripcion")]
        public string? Descripcion { get; set; }
    }
}