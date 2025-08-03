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
        public string Vpd { get; set; }

        [Required]
        [MaxLength(255)]
        public string NombrePortal { get; set; }

        [Required]
        [MaxLength(255)]
        public string Logo { get; set; }
    }
}