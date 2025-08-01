namespace SubvencionesApp.Application.Dtos
{
    public class DatosEstadisticosDto
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public int? TotalConcesiones { get; set; }
        public decimal? ImporteTotal { get; set; }
    }
}