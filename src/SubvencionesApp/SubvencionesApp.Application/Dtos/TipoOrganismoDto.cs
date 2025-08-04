using System;

namespace SubvencionesApp.Application.Dtos
{
    public class TipoOrganismoDto
    {
        public Guid Id { get; set; }
        public int? ExternalId { get; set; }
        public string? Descripcion { get; set; }
    }
}