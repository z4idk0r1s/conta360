using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class SuscripcionApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("fechaInicio")]
        public string FechaInicio { get; set; }

        [JsonProperty("activa")]
        public bool Activa { get; set; }
    }
}