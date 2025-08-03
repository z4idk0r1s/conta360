using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class OrganosCodigoAdmin
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string CodigoAdmin { get; set; }

        [Required]
        [MaxLength(255)]
        public string Nombre { get; set; }
    }
}