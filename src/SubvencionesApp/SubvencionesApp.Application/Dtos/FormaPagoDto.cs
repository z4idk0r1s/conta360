using System;

namespace SubvencionesApp.Application.Dtos
{
    public class FormaPagoDto
    {
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }
        public string? Descripcion { get; set; }
    }
}