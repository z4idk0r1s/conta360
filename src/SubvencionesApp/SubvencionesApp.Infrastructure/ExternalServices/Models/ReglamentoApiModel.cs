using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class ReglamentoApiModel
    {
        [JsonProperty("id")]
        public required string Id { get; set; }

        [JsonProperty("nombre")]
        public required string Nombre { get; set; }

        [JsonProperty("tipo")]
        public required string Tipo { get; set; }
    }
}