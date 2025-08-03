using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class AccionApiModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("descripcion")]
        public required string Descripcion { get; set; }
    }
}