using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class SuscripcionApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public required string Nombre { get; set; }

        [JsonProperty("email")]
        public required string Email { get; set; }

        [JsonProperty("fechaInicio")]
        public required string FechaInicio { get; set; }

        [JsonProperty("activa")]
        public bool Activa { get; set; }
    }
}