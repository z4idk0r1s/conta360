using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class ProgramaApiModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("codigo")]
        public string? Codigo { get; set; }

        [JsonProperty("descripcion")]
        public string? Descripcion { get; set; }
    }
}