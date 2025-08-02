using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class PartidoPoliticoApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("importe")]
        public decimal Importe { get; set; }

        [JsonProperty("fecha")]
        public string Fecha { get; set; }

        [JsonProperty("organismoId")]
        public string OrganismoId { get; set; }
    }
}