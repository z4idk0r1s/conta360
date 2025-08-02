using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class PlanEstrategicoApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; }

        [JsonProperty("fechaAprobacion")]
        public string FechaAprobacion { get; set; }
    }
}