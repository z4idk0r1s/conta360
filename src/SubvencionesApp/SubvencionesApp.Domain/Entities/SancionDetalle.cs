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
        public string? Nombre { get; set; }

        [Required]
        public string? Motivo { get; set; }

        [Required]
        public string? SancionTexto { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string? Estado { get; set; }

        [Required]
        public string? Detalles { get; set; }

        [Required]
        public DateTime FechaResolucion { get; set; }

        [Required]
        public Guid OrganismoId { get; set; }
    }
}