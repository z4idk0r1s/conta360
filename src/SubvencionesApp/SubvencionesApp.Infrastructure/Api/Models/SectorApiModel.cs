using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.Api.Models
{
    public class SectorApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("descripcion")]
        public string? Descripcion { get; set; }
    }
}