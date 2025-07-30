using Newtonsoft.Json;
using System;

namespace SubvencionesApp.Infrastructure.Api.Models
{
    public class ConvocatoriaApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("referenciaBDNS")]
        public string? ReferenciaBDNS { get; set; }

        [JsonProperty("ejercicio")]
        public int? Ejercicio { get; set; }

        [JsonProperty("fechaPublicacion")]
        public DateTime? FechaPublicacion { get; set; }

        [JsonProperty("enlace")]
        public string? Enlace { get; set; }

        [JsonProperty("extracto")]
        public string? Extracto { get; set; }

        [JsonProperty("objeto")]
        public string? Objeto { get; set; }

        [JsonProperty("organismo")]
        public OrganismoApiModel? Organismo { get; set; }

        [JsonProperty("situacionEntorno")]
        public SituacionEntornoApiModel? SituacionEntorno { get; set; }

        [JsonProperty("tipoConvocatoria")]
        public TipoConvocatoriaApiModel? TipoConvocatoria { get; set; }

        [JsonProperty("tipoSubvencion")]
        public TipoSubvencionApiModel? TipoSubvencion { get; set; }
    }
}