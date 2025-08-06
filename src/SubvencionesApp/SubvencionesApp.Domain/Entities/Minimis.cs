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
        public string? Nombre { get; set; }

        [Required]
        public string? Descripcion { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Estado { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }
        
        public DateTime? FechaFin { get; set; }
    }
}