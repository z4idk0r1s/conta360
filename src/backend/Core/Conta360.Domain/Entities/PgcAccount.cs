using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Conta360.Domain.Entities
{
    /// <summary>
    /// Representa una cuenta del Plan General Contable (PGC).
    /// Puede ser un grupo, subgrupo, cuenta o subcuenta, según su nivel.
    /// </summary>
    public class PgcAccount : BaseEntity
    {   
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Código completo de la cuenta (por ejemplo, "5701", "476.01", etc.).
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Nombre o descripción de la cuenta.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Código de la cuenta padre (por ejemplo, padre de "5701" es "570").
        /// </summary>
        [MaxLength(10)]
        public string? ParentCode { get; set; } = string.Empty;

        /// <summary>
        /// Indica si esta cuenta acumula saldos (true para cuentas y subcuentas, false para grupos/subgrupos).
        /// </summary>
        public bool IsMovable { get; set; }

        public int? ParentId { get; set; }
        
        /// <summary>
        /// Nivel jerárquico: 1 = Grupo, 2 = Subgrupo, 3 = Cuenta, 4 = Subcuenta.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Navegación a la entidad padre (no se almacena en BD, se resuelve al cargar).
        /// </summary>
        [ForeignKey("ParentId")]
        public PgcAccount? Parent { get; set; }

        /// <summary>
        /// Hija(s) directas en la jerarquía (subcuentas o sub-subgrupos).
        /// </summary>
        public ICollection<PgcAccount> Children { get; set; } = new List<PgcAccount>();
    }
}
