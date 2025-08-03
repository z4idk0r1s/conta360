using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class GrandeBeneficiarioApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public required string Nombre { get; set; }

        [JsonProperty("tipo")]
        public required string Tipo { get; set; }

        [JsonProperty("importe")]
        public decimal Importe { get; set; }
    }
}