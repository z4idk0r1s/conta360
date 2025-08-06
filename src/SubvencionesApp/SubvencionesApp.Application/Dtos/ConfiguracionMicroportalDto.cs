using System;

namespace SubvencionesApp.Application.Dtos
{
    public class ConfiguracionMicroportalDto
    {
        public Guid Id {get; set;}
        public int? ExternalId { get; set; }
        public string? Vpd { get; set; }
        public string? NombrePortal { get; set; }
        public string? Logo { get; set; }
    }
}