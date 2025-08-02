using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class ActividadApiModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }
    }
}