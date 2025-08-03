using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class PartidoPolitico
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        
        public int? ExternalId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Nombre { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Importe { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public Guid OrganismoId { get; set; }
    }
}