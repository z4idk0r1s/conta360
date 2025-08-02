namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class SuscripcionApiModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string FechaInicio { get; set; }
        public bool Activa { get; set; }
    }
}