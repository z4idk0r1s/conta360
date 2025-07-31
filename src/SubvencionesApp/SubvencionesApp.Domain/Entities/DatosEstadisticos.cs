using System.ComponentModel.DataAnnotations;

namespace SubvencionesApp.Domain.Entities
{
    public class DatosEstadisticos
    {
        public string? Descripcion { get; set; }
        public int? TotalConcesiones { get; set; }
        public decimal? ImporteTotal { get; set; }
    }
}