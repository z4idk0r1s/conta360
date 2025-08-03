using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class Suscripcion
    {
        [Key]
        public Guid Id { get; set; }

        public int? ExternalId { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string? Nombre { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Email { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public bool Activa { get; set; }
    }
}