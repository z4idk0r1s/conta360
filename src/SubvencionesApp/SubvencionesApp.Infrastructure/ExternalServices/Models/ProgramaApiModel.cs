using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class ProgramaApiModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("codigo")]
        public required string Codigo { get; set; }

        [JsonProperty("descripcion")]
        public required string Descripcion { get; set; }
    }
}