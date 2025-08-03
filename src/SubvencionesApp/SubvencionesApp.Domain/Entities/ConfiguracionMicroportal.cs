using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubvencionesApp.Domain.Entities
{
    public class ConfiguracionMicroportal
    {
        [Key]
        [Required]
        [MaxLength(10)]
        public required string Vpd { get; set; }

        [Required]
        [MaxLength(255)]
        public required string NombrePortal { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Logo { get; set; }
    }
}