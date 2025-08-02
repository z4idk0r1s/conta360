using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class SancionDetalleApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("motivo")]
        public string Motivo { get; set; }

        [JsonProperty("sancion")]
        public string Sancion { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; }

        [JsonProperty("detalles")]
        public string Detalles { get; set; }

        [JsonProperty("fechaResolucion")]
        public string FechaResolucion { get; set; }

        [JsonProperty("organismoId")]
        public string OrganismoId { get; set; }
    }
}