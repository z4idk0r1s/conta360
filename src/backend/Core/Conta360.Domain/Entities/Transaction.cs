using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Conta360.Domain.Entities
{
    public class Transact : BaseEntity
    {
        public Account? Account { get; set; }
        
        public int AccountId { get; set; }

        [Required]
        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int PgcAccountId { get; set; }

        [ForeignKey("PgcAccountId")]
        public PgcAccount? PgcAccount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
    }
}
