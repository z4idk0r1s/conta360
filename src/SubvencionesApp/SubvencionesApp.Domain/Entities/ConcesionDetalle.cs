using System;

namespace SubvencionesApp.Domain.Entities
{
    public class ConcesionDetalle
    {
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Detalles { get; set; }
        public decimal Importe { get; set; }
        public string BeneficiarioId { get; set; }
        public string OrganismoId { get; set; }
        public string FechaResolucion { get; set; }
    }
}