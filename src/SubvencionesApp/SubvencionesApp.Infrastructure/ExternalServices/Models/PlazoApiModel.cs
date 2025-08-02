using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class PlazoApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("fechaInicio")]
        public string FechaInicio { get; set; }

        [JsonProperty("fechaFin")]
        public string FechaFin { get; set; }

        [JsonProperty("convocatoriaId")]
        public string ConvocatoriaId { get; set; }
    }
}