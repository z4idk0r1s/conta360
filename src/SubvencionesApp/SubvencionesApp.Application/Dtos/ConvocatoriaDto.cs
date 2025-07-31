namespace SubvencionesApp.Application.Dtos
{
    public class ConvocatoriaDto
    {
        public int Id { get; set; }
        public string? Objeto { get; set; }
        public string? Extracto { get; set; }
        public string? Enlace { get; set; }
        public string? ReferenciaBDNS { get; set; }
        public int? Ejercicio { get; set; }
        public DateTime? FechaPublicacion { get; set; }
    }
}