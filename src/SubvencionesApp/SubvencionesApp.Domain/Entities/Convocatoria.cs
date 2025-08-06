using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class Convocatoria
    {
        [Key]
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }

        [MaxLength(255)]
        public string? Objeto { get; set; }

        [MaxLength(255)]
        public string? Extracto { get; set; }

        [MaxLength(255)]
        public string? Enlace { get; set; }

        [MaxLength(255)]
        public string? ReferenciaBDNS { get; set; }

        public int? Ejercicio { get; set; }

        public DateTime? FechaPublicacion { get; set; }

        // Relaciones
        public Guid? TipoConvocatoriaId { get; set; }
        public TipoConvocatoria? TipoConvocatoria { get; set; }

        public Guid? TipoSubvencionId { get; set; }
        public TipoSubvencion? TipoSubvencion { get; set; }

        public Guid? OrganismoId { get; set; }
        public Organismo? Organismo { get; set; }

        public Guid? SituacionEntornoId { get; set; }
        public SituacionEntorno? SituacionEntorno { get; set; }

        public ICollection<Concesion> Concesiones { get; set; } = new List<Concesion>();
    }
}