using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class Beneficiario
    {
        [Key]
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Nombre { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Tipo { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Identificacion { get; set; }
        // Propiedad de navegación inversa para la relación con Concesion
        public ICollection<Concesion> Concesiones { get; set; } = new List<Concesion>();

        // Propiedad de navegación inversa para la relación con ConcesionDetalle
        public ICollection<ConcesionDetalle> ConcesionesDetalle { get; set; } = new List<ConcesionDetalle>();
    }
}