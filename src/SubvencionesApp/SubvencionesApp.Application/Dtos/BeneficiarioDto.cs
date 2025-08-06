using System;

namespace SubvencionesApp.Application.Dtos
{
    public class BeneficiarioDto
    {
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }
        public string? Nombre { get; set; }
        public string? Identificacion { get; set; }
    }
}