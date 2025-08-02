using System;
using System.Collections.Generic;

namespace SubvencionesApp.Application.Dtos
{
    public class ConvocatoriaDetalleDto
    {
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Detalles { get; set; }
        public string Estado { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string FechaPublicacion { get; set; }
        public string OrganismoId { get; set; }
        public string RegionId { get; set; }
        public string TipoBeneficiarioId { get; set; }
        public string InstrumentoId { get; set; }
        public List<PlazoDto> Plazos { get; set; } = new List<PlazoDto>();
        public object Documentos { get; set; }
        public List<BeneficiarioDto> Beneficiarios { get; set; } = new List<BeneficiarioDto>();
    }
}