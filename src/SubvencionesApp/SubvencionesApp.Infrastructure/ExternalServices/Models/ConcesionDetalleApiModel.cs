using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class ConcesionDetalleApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("detalles")]
        public string Detalles { get; set; }

        [JsonProperty("importe")]
        public decimal Importe { get; set; }

        [JsonProperty("beneficiarioId")]
        public string BeneficiarioId { get; set; }

        [JsonProperty("organismoId")]
        public string OrganismoId { get; set; }

        [JsonProperty("fechaResolucion")]
        public string FechaResolucion { get; set; }
    }
}