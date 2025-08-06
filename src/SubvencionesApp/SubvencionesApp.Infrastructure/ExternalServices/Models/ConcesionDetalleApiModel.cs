using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class ConcesionDetalleApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public required string Nombre { get; set; }

        [JsonProperty("descripcion")]
        public required string Descripcion { get; set; }

        [JsonProperty("detalles")]
        public required string Detalles { get; set; }

        [JsonProperty("importe")]
        public decimal Importe { get; set; }

        [JsonProperty("beneficiarioId")]
        public required string BeneficiarioId { get; set; }

        [JsonProperty("organismoId")]
        public required string OrganismoId { get; set; }

        [JsonProperty("fechaResolucion")]
        public required string FechaResolucion { get; set; }
    }
}