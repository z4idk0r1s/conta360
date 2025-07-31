using System.ComponentModel.DataAnnotations;

namespace SubvencionesApp.Core.Entities
{
    public class DatosEstadisticos
    {
        public string? Descripcion { get; set; }
        public int? TotalConcesiones { get; set; }
        public decimal? ImporteTotal { get; set; }
    }
}