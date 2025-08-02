namespace SubvencionesApp.Infrastructure.ExternalServices.Models
{
    public class PlazoApiModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string ConvocatoriaId { get; set; }
    }
}