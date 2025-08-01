using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class Beneficiario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? Id { get; set; }

        [MaxLength(255)]
        public string? Nombre { get; set; }

        [MaxLength(255)]
        public string? Identificacion { get; set; }

        public ICollection<Concesion> Concesiones { get; set; } = new List<Concesion>();
    }
}