using System;

namespace SubvencionesApp.Domain.Entities
{
    public class AyudaEstado
    {
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string InstrumentoId { get; set; }
        public string TipoBeneficiarioId { get; set; }
        public string Estado { get; set; }
    }
}