using System;

namespace SubvencionesApp.Application.Dtos
{
    public class OrganosCodigoAdminDto
    {
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }
        public string? CodigoAdmin { get; set; }
        public string? Nombre { get; set; }
    }
}