using System;

namespace SubvencionesApp.Application.Dtos
{
    public class SituacionEntornoDto
    {
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }
        public string? Descripcion { get; set; }
    }
}