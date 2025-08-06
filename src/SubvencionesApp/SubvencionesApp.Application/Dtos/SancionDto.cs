using System;

namespace SubvencionesApp.Application.Dtos
{
    public class SancionDto
    {
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public string? Nombre { get; set; }
        public string? Motivo { get; set; }
        public string? Estado { get; set; }
    }
}