using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class Ayuda
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        public int? ExternalId { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Nombre { get; set; }

        [Required]
        public string? Descripcion { get; set; }

        [Required]
        public Guid OrganismoId { get; set; }

        [Required]
        public Guid RegionId { get; set; }

        [Required]
        public Guid TipoBeneficiarioId { get; set; }

        [Required]
        public Guid InstrumentoId { get; set; }
    }
}