using System;

namespace SubvencionesApp.Domain.Entities
{
    public class GrandeBeneficiario
    {
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public decimal Importe { get; set; }
    }
}