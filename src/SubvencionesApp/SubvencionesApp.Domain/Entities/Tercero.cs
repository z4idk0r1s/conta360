using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class Tercero
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        
        public int? ExternalId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(20)]
        public string Nif { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Tipo { get; set; }
    }
}