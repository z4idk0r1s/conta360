using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class ConcesionDetalle
    {
        [Key]
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
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Importe { get; set; }

        // Claves foráneas
        [Required]
        [ForeignKey("Beneficiario")]
        public Guid BeneficiarioId { get; set; }
        public Beneficiario? Beneficiario { get; set; }

        [Required]
        [ForeignKey("Organismo")]
        public Guid OrganismoId { get; set; }
        public Organismo? Organismo { get; set; }

        [Required]
        public DateTime FechaResolucion { get; set; }
    }
}