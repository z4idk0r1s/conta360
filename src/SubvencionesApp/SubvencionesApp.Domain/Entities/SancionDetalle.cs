using System;

namespace SubvencionesApp.Domain.Entities
{
    public class SancionDetalle
    {
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public string Nombre { get; set; }
        public string Motivo { get; set; }
        public string Sancion { get; set; }
        public string Estado { get; set; }
        public string Detalles { get; set; }
        public string FechaResolucion { get; set; }
        public string OrganismoId { get; set; }
    }
}