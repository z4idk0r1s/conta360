using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class ReglamentoApiModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("tipo")]
        public string Tipo { get; set; }
    }
}