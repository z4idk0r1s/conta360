using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class TerceroApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("nif")]
        public string Nif { get; set; }

        [JsonProperty("tipo")]
        public string Tipo { get; set; }
    }
}