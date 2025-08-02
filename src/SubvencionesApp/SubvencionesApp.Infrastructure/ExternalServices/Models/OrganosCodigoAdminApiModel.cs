using Newtonsoft.Json;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class OrganosCodigoAdminApiModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("codigoAdmin")]
        public string CodigoAdmin { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }
    }
}