using System;

namespace SubvencionesApp.Domain.Entities
{
    public class EnlaceMicroVentana
    {
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public string Nombre { get; set; }
        public string Url { get; set; }
    }
}