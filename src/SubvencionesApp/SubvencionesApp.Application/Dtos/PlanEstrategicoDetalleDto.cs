using System;
using System.Collections.Generic;

namespace SubvencionesApp.Application.Dtos
{
    public class PlanEstrategicoDetalleDto
    {
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? Estado { get; set; }
        public string? FechaAprobacion { get; set; }
        public List<ObjetivoDto> Objetivos { get; set; } = new List<ObjetivoDto>();
    }
}