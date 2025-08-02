using System.Collections.Generic;

namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class PlanEstrategicoDetalleApiModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string FechaAprobacion { get; set; }
        public List<ObjetivoApiModel> Objetivos { get; set; }
    }
}