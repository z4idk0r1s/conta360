using System;

namespace SubvencionesApp.Application.Dtos
{
    public class ReglamentoDto
    {
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }       
        public string? Nombre { get; set; }
        public string? Tipo { get; set; }
    }
}
