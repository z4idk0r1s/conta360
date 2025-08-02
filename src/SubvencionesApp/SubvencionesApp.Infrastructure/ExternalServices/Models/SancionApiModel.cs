namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class SancionApiModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Motivo { get; set; }
        public string Sancion { get; set; }
        public string Estado { get; set; }
    }
}