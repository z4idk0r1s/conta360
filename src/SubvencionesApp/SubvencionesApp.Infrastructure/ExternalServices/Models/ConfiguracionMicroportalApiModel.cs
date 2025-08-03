using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class ConfiguracionMicroportalApiModel
    {
        // En este caso, no hay un 'Id' específico, por lo que las propiedades se mapean directamente
        [JsonProperty("vpd")]
        public required string Vpd { get; set; }

        [JsonProperty("nombrePortal")]
        public required string NombrePortal { get; set; }

        [JsonProperty("logo")]
        public required string Logo { get; set; }
    }
}