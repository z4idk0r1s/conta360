using System;

namespace SubvencionesApp.Application.Dtos
{
    public class RegionDto
    {
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }
        public string? Nombre { get; set; }
    }
}