using Newtonsoft.Json;
using System;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class ConcesionApiModel
    {
        [JsonProperty("idConcesion")]
        public long IdConcesion { get; set; }

        [JsonProperty("referenciaBDNS")]
        public required string ReferenciaBDNS { get; set; }

        [JsonProperty("referenciaPublicacion")]
        public required string ReferenciaPublicacion { get; set; }

        [JsonProperty("importe")]
        public decimal? Importe { get; set; }

        [JsonProperty("ejercicio")]
        public int? Ejercicio { get; set; }

        [JsonProperty("fechaConcesion")]
        public DateTime? FechaConcesion { get; set; }

        [JsonProperty("beneficiario")]
        public BeneficiarioApiModel? Beneficiario { get; set; }

        [JsonProperty("convocatoria")]
        public ConvocatoriaApiModel? Convocatoria { get; set; }
    }
}