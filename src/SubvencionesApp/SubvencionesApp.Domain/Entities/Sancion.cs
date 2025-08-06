using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class Sancion
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
    }
}