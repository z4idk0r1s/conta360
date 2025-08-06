using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class PlazoApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public required string Nombre { get; set; }

        [JsonProperty("fechaInicio")]
        public required string FechaInicio { get; set; }

        [JsonProperty("fechaFin")]
        public required string FechaFin { get; set; }

        [JsonProperty("convocatoriaId")]
        public required string ConvocatoriaId { get; set; }
    }
}