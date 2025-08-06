using System;

namespace SubvencionesApp.Application.Dtos
{
    public class TramoDto
    {
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }
        public string? Descripcion { get; set; }
    }
}