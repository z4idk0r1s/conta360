using System;

namespace SubvencionesApp.Application.Dtos
{
    public class TerceroDto
    {
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public string? Nombre { get; set; }
        public string? Nif { get; set; }
        public string? Tipo { get; set; }
    }
}