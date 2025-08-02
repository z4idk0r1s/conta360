using System;

namespace SubvencionesApp.Domain.Entities
{
    public class Sancion
    {
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public string Nombre { get; set; }
        public string Motivo { get; set; }
        public string Sancion { get; set; }
        public string Estado { get; set; }
    }
}