using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class Minimis
    {
        [Key]
        public Guid Id { get; set; }
        
        public int? ExternalId { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Nombre { get; set; }

        [Required]
        public required string Descripcion { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Estado { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }
        
        public DateTime? FechaFin { get; set; }
    }
}