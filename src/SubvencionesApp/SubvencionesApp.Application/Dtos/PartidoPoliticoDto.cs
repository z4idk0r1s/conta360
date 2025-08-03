using System;

namespace SubvencionesApp.Application.Dtos
{
    public class PartidoPoliticoDto
    {
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }
        public string? Nombre { get; set; }
        public decimal? Importe { get; set; }
        public string? Fecha { get; set; }
        public string? OrganismoId { get; set; }
    }
}