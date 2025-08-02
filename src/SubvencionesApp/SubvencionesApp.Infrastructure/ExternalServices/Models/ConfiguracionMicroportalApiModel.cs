using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class ConfiguracionMicroportalApiModel
    {
        // En este caso, no hay un 'Id' específico, por lo que las propiedades se mapean directamente
        [JsonProperty("vpd")]
        public string Vpd { get; set; }

        [JsonProperty("nombrePortal")]
        public string NombrePortal { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }
    }
}