using System;

namespace SubvencionesApp.Application.Dtos
{
    public class DatosEstadisticosDto
    {
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }
        public string? Descripcion { get; set; }
        public int? TotalConcesiones { get; set; }
        public decimal? ImporteTotal { get; set; }
    }
}