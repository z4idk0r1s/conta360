namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class MinimisApiModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
    }
}