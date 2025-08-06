using System;
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

        [Required]
        public Guid BeneficiarioId { get; set; }

        [Required]
        public Guid OrganismoId { get; set; }

        [Required]
        public DateTime FechaResolucion { get; set; }
    }
}