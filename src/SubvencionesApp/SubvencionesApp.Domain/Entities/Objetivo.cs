using System;

namespace SubvencionesApp.Domain.Entities
{
    public class Objetivo
    {
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}