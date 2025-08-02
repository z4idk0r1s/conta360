using System;

namespace SubvencionesApp.Domain.Entities
{
    public class Tercero
    {
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public string Nombre { get; set; }
        public string Nif { get; set; }
        public string Tipo { get; set; }
    }
}