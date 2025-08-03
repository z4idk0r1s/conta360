using System;

namespace SubvencionesApp.Application.Dtos
{
    public class MinimisDto
    {
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? Estado { get; set; }
        public string? FechaInicio { get; set; }
        public string? FechaFin { get; set; }
    }
}