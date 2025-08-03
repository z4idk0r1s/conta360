using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class ConvocatoriaDetalle
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        
        public int? ExternalId { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Nombre { get; set; }

        [Required]
        public string? Descripcion { get; set; }

        [Required]
        public string? Detalles { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Estado { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Required]
        public DateTime FechaPublicacion { get; set; }

        [Required]
        public Guid OrganismoId { get; set; }

        [Required]
        public Guid RegionId { get; set; }

        [Required]
        public Guid TipoBeneficiarioId { get; set; }

        [Required]
        public Guid InstrumentoId { get; set; }

        public ICollection<Plazo>? Plazos { get; set; }

        public string? Documentos { get; set; }

        public ICollection<Beneficiario>? Beneficiarios { get; set; }
    }
}