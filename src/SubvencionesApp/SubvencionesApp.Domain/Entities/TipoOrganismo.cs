using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class TipoOrganismo
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(255)]
        public string? Descripcion { get; set; }
    }
}