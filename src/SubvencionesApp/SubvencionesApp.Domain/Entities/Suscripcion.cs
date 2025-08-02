using System;

namespace SubvencionesApp.Domain.Entities
{
    public class Suscripcion
    {
        public Guid Id { get; set; }
        public int ExternalId { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string FechaInicio { get; set; }
        public bool Activa { get; set; }
    }
}