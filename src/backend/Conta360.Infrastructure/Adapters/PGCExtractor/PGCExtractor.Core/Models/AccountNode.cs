using System.Collections.Generic;

namespace Conta360.Infrastructure.Adapters.PGCExtractor.PGCExtractor.Core.Models
{
    public class AccountNode
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // Clase, Grupo, Subgrupo, Cuenta, Subcuenta
        public string ParentCode { get; set; } // Relación jerárquica
        public string Category { get; set; } // Activo, Pasivo, etc.
        public List<AccountNode> Children { get; set; } = new();
    }
}
