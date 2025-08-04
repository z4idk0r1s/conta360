using System;

namespace SubvencionesApp.Application.Dtos
{
    public class EntidadDto
    {
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }
        public string? Descripcion { get; set; }
    }
}