using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class FinalidadApiModel
    {
        [JsonProperty("id")]
        public required string Id { get; set; }

        [JsonProperty("nombre")]
        public required string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public required string Descripcion { get; set; }
    }
}