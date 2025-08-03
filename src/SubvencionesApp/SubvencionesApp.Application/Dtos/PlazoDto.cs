using System;

namespace SubvencionesApp.Application.Dtos
{
    public class PlazoDto
    {
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public string? Nombre { get; set; }
        public string? FechaInicio { get; set; }
        public string? FechaFin { get; set; }
        public string? ConvocatoriaId { get; set; }
    }
}