using System;

namespace SubvencionesApp.Application.Dtos
{
    public class FinalidadDto
    {
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
    }
}
