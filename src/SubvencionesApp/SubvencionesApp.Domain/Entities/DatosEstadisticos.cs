using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class DatosEstadisticos
    {
        [Key]
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }

        public string? Descripcion { get; set; }

        public int? TotalConcesiones { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ImporteTotal { get; set; }
    }
}