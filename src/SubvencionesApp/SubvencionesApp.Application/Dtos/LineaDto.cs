using System;

namespace SubvencionesApp.Application.Dtos
{
    public class LineaDto
    {
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }
        public string? Codigo { get; set; }
        public string? Nombre { get; set; }
    }
}