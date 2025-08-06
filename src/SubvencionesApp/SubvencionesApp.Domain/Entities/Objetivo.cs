using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class Objetivo
    {
        [Key]
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Nombre { get; set; }

        [Required]
        public string? Descripcion { get; set; }
        
        // Propiedad de clave foránea y de navegación 
        [ForeignKey("PlanEstrategicoDetalle")]
        public Guid? PlanEstrategicoDetalleId { get; set; }
        public PlanEstrategicoDetalle? PlanEstrategicoDetalle { get; set; }
    }
}