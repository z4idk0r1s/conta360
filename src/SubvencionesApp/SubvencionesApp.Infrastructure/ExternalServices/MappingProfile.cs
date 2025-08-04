using AutoMapper;
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Infrastructure.ExternalServices.Models;
using SubvencionesApp.Infrastructure.Mappings;
using System;

namespace SubvencionesApp.Infrastructure.ExternalServices
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // 1. Conversiones globales seguras y reutilizables
            ConfigureGlobalConversions();

            // 2. Mapeos de API a DTOs (importación de datos)
            ConfigureApiToDtoMappings();

            // 3. Mapeos de DTO a Entidad para creación y actualización
            ConfigureDtoToEntityMappings();

            // 4. Mapeos de Entidad a DTO para lectura y consultas
            ConfigureEntityToDtoMappings();
        }
    }
}