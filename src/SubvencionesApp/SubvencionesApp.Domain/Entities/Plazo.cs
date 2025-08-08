using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class Plazo
    {
        [Key]
        public Guid Id { get; set; }

        public int? ExternalId { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Nombre { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        // Propiedad de clave foránea y de navegación
        [Required]
        [ForeignKey("Convocatoria")]
        public Guid ConvocatoriaId { get; set; }
        public Convocatoria? Convocatoria { get; set; }
    }
}