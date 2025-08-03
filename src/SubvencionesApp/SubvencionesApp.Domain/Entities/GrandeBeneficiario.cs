using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class GrandeBeneficiario
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        
        public int? ExternalId { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Nombre { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Tipo { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Importe { get; set; }
    }
}