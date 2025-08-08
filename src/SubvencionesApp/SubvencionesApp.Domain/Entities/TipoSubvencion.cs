using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class TipoSubvencion
    {
        [Key]
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }

        [MaxLength(255)]
        public string? Descripcion { get; set; }

        // Propiedad de navegación inversa
        public ICollection<Convocatoria> Convocatorias { get; set; } = new List<Convocatoria>();
    }
}