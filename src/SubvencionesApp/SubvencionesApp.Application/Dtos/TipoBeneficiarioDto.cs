using System;

namespace SubvencionesApp.Application.Dtos
{
    public class TipoBeneficiarioDto
    {
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }
        public string? Descripcion { get; set; }
        public string? Nombre { get; set; }
    }
}
