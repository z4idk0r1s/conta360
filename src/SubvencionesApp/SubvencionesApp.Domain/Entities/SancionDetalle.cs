using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class SancionDetalle
    {
        [Key]
        public Guid Id { get; set; }

        public int? ExternalId { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Nombre { get; set; }

        [Required]
        public required string Motivo { get; set; }

        [Required]
        public required string SancionTexto { get; set; }
        
        [Required]
        [MaxLength(50)]
        public required string Estado { get; set; }

        [Required]
        public required string Detalles { get; set; }

        [Required]
        public DateTime FechaResolucion { get; set; }

        [Required]
        public Guid OrganismoId { get; set; }
    }
}