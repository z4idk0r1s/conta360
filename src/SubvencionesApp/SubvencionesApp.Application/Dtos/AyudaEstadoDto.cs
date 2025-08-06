using System;

namespace SubvencionesApp.Application.Dtos
{
    public class AyudaEstadoDto
    {
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? InstrumentoId { get; set; }
        public string? TipoBeneficiarioId { get; set; }
        public string? Estado { get; set; }
    }
}