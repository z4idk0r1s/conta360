using System;

namespace SubvencionesApp.Application.Dtos
{
    public class EnlaceMicroVentanaDto
    {
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }
        public string? Nombre { get; set; }
        public string? Url { get; set; }
    }
}