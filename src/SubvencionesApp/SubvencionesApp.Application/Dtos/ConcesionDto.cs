namespace SubvencionesApp.Application.Dtos
{
    public class ConcesionDto
    {
        public long IdConcesion { get; set; }
        public string? ReferenciaBDNS { get; set; }
        public string? ReferenciaPublicacion { get; set; }
        public decimal? Importe { get; set; }
        public int? Ejercicio { get; set; }
        public DateTime? FechaConcesion { get; set; }
        public int? BeneficiarioId { get; set; }
        public int? ConvocatoriaId { get; set; } 
    }
}