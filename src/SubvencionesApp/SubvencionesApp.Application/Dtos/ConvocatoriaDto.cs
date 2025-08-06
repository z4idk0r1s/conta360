using System;

namespace SubvencionesApp.Application.Dtos
{
    public class ConvocatoriaDto
    {
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }
        public string? Objeto { get; set; }
        public string? Extracto { get; set; }
        public string? Enlace { get; set; }
        public string? ReferenciaBDNS { get; set; }
        public int? Ejercicio { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public int? TipoConvocatoriaId { get; set; }
        public int? TipoSubvencionId { get; set; }
        public int? OrganismoId { get; set; }
        public int? SituacionEntornoId { get; set; }

    }
}