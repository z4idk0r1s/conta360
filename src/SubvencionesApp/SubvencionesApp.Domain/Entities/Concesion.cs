using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class Concesion
    {
        [Key]
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }

        public long IdConcesion { get; set; }

        [MaxLength(255)]
        public string? ReferenciaBDNS { get; set; }

        [MaxLength(255)]
        public string? ReferenciaPublicacion { get; set; }

        public decimal? Importe { get; set; }

        public int? Ejercicio { get; set; }

        public DateTime? FechaConcesion { get; set; }

        // Relaciones
        [ForeignKey("Beneficiario")]
        public Guid? BeneficiarioId { get; set; }
        public Beneficiario? Beneficiario { get; set; }
        
        [ForeignKey("Convocatoria")]
        public Guid? ConvocatoriaId { get; set; }
        public Convocatoria? Convocatoria { get; set; }
    }
}