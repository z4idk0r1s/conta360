using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class Tercero
    {
        [Key]
        public Guid Id { get; set; }
        
        public int? ExternalId { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Nombre { get; set; }

        [Required]
        [MaxLength(20)]
        public required string Nif { get; set; }
        
        [Required]
        [MaxLength(50)]
        public required string Tipo { get; set; }
    }
}