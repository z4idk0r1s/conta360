using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class Agrupacion
    {
        [Key]
        public Guid Id { get; set; }

        public int? ExternalId { get; set; }

        [MaxLength(255)]
        public string? Nombre { get; set; }

        [MaxLength(255)]
        public string? Descripcion { get; set; }
    }
}