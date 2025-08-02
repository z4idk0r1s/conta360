namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class PlanEstrategicoApiModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string FechaAprobacion { get; set; }
    }
}