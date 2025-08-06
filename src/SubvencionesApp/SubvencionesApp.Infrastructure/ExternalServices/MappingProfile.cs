using AutoMapper;
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Infrastructure.ExternalServices.Models;
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

        // ===========================================
        // 1. Conversiones globales seguras
        // ===========================================
        private void ConfigureGlobalConversions()
        {
            // Estos mapeos son seguros y robustos, ya que usan los métodos estáticos de MappingHelpers
            // para evitar excepciones con datos externos malformados y el error del Expression Tree.
            CreateMap<Guid, string>().ConvertUsing(src => src.ToString());
            CreateMap<Guid?, string>().ConvertUsing(src => src.HasValue ? src.Value.ToString() : string.Empty);
            CreateMap<string, Guid>().ConvertUsing(src => MappingHelpers.StringToGuid(src));
            CreateMap<string, Guid?>().ConvertUsing(src => MappingHelpers.StringToNullableGuid(src));

            CreateMap<DateTime, string>().ConvertUsing(src => src.ToString("yyyy-MM-dd"));
            CreateMap<DateTime?, string>().ConvertUsing(src => src.HasValue ? src.Value.ToString("yyyy-MM-dd") : string.Empty);
            CreateMap<string, DateTime>().ConvertUsing(src => MappingHelpers.StringToDateTime(src));
            CreateMap<string, DateTime?>().ConvertUsing(src => MappingHelpers.StringToNullableDateTime(src));
            
            CreateMap<decimal, decimal?>().ConvertUsing(src => src);
            CreateMap<decimal?, decimal>().ConvertUsing(src => src ?? 0m);
            CreateMap<int, int?>().ConvertUsing(src => src);
            CreateMap<int?, int>().ConvertUsing(src => src ?? 0);
        }

        // ===========================================
        // 2. API → DTO
        // ===========================================
        private void ConfigureApiToDtoMappings()
        {
            // Mapeos de modelos de la API a DTOs.
            // La propiedad 'Id' de la API se mapea a 'ExternalId' en el DTO.
            CreateMap<ConvocatoriaApiModel, ConvocatoriaDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<ConvocatoriaDetalleApiModel, ConvocatoriaDetalleDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
             CreateMap<ConcesionApiModel, ConcesionDto>().ForMember(dest => dest.IdConcesion, opt => opt.MapFrom(src => src.IdConcesion));
            CreateMap<ConcesionDetalleApiModel, ConcesionDetalleDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<AyudaApiModel, AyudaDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<AyudaEstadoApiModel, AyudaEstadoDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<PlazoApiModel, PlazoDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<MinimisApiModel, MinimisDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<GrandeBeneficiarioApiModel, GrandeBeneficiarioDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<PartidoPoliticoApiModel, PartidoPoliticoDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<PlanEstrategicoApiModel, PlanEstrategicoDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<PlanEstrategicoDetalleApiModel, PlanEstrategicoDetalleDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<SancionApiModel, SancionDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<SancionDetalleApiModel, SancionDetalleDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<TerceroApiModel, TerceroDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<RegionApiModel, RegionDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<FinalidadApiModel, FinalidadDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<TipoBeneficiarioApiModel, TipoBeneficiarioDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<InstrumentoApiModel, InstrumentoDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<ReglamentoApiModel, ReglamentoDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<SectorProductoApiModel, SectorProductoDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<ActividadApiModel, ActividadDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<ObjetivoApiModel, ObjetivoDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<OrganismoApiModel, OrganismoDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<OrganosCodigoAdminApiModel, OrganosCodigoAdminDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<ConfiguracionMicroportalApiModel, ConfiguracionMicroportalDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<EnlaceMicroVentanaApiModel, EnlaceMicroVentanaDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<SuscripcionApiModel, SuscripcionDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<BeneficiarioApiModel, BeneficiarioDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
        }

        // ===========================================
        // 3. DTO → Entidad (Creación y actualización)
        // ===========================================
        private void ConfigureDtoToEntityMappings()
        {
            // Mapeos unidireccionales de DTO a Entidad para crear o actualizar.
            CreateMap<ConvocatoriaDto, Convocatoria>();
            CreateMap<ConvocatoriaDetalleDto, ConvocatoriaDetalle>();
            CreateMap<ConcesionDto, Concesion>();
            CreateMap<ConcesionDetalleDto, ConcesionDetalle>();
            CreateMap<AyudaDto, Ayuda>();
            CreateMap<AyudaEstadoDto, AyudaEstado>();
            CreateMap<PlazoDto, Plazo>();
            CreateMap<MinimisDto, Minimis>();
            CreateMap<GrandeBeneficiarioDto, GrandeBeneficiario>();
            CreateMap<PartidoPoliticoDto, PartidoPolitico>();
            CreateMap<PlanEstrategicoDto, PlanEstrategico>();
            CreateMap<PlanEstrategicoDetalleDto, PlanEstrategicoDetalle>();
            CreateMap<SancionDto, Sancion>();
            CreateMap<SancionDetalleDto, SancionDetalle>();
            CreateMap<TerceroDto, Tercero>();
            CreateMap<RegionDto, Region>();
            CreateMap<FinalidadDto, Finalidad>();
            CreateMap<TipoBeneficiarioDto, TipoBeneficiario>();
            CreateMap<InstrumentoDto, Instrumento>();
            CreateMap<ReglamentoDto, Reglamento>();
            CreateMap<SectorProductoDto, SectorProducto>();
            CreateMap<ActividadDto, Actividad>();
            CreateMap<ObjetivoDto, Objetivo>();
            CreateMap<OrganismoDto, Organismo>();
            CreateMap<OrganosCodigoAdminDto, OrganosCodigoAdmin>();
            CreateMap<ConfiguracionMicroportalDto, ConfiguracionMicroportal>();
            CreateMap<EnlaceMicroVentanaDto, EnlaceMicroVentana>();
            CreateMap<SuscripcionDto, Suscripcion>();
            CreateMap<BeneficiarioDto, Beneficiario>();
        }

        // ===========================================
        // 4. Entidad → DTO (Lectura y consultas)
        // ===========================================
        private void ConfigureEntityToDtoMappings()
        {
            // Mapeos unidireccionales de Entidad a DTO.
            // Es la forma más óptima para usar con 'ProjectTo<T>' de EF Core.
            CreateMap<Convocatoria, ConvocatoriaDto>();
            CreateMap<ConvocatoriaDetalle, ConvocatoriaDetalleDto>();
            CreateMap<Concesion, ConcesionDto>();
            CreateMap<ConcesionDetalle, ConcesionDetalleDto>();
            CreateMap<Ayuda, AyudaDto>();
            CreateMap<AyudaEstado, AyudaEstadoDto>();
            CreateMap<Plazo, PlazoDto>();
            CreateMap<Minimis, MinimisDto>();
            CreateMap<GrandeBeneficiario, GrandeBeneficiarioDto>();
            CreateMap<PartidoPolitico, PartidoPoliticoDto>();
            CreateMap<PlanEstrategico, PlanEstrategicoDto>();
            CreateMap<PlanEstrategicoDetalle, PlanEstrategicoDetalleDto>();
            CreateMap<Sancion, SancionDto>();
            CreateMap<SancionDetalle, SancionDetalleDto>();
            CreateMap<Tercero, TerceroDto>();
            CreateMap<Region, RegionDto>();
            CreateMap<Finalidad, FinalidadDto>();
            CreateMap<TipoBeneficiario, TipoBeneficiarioDto>();
            CreateMap<Instrumento, InstrumentoDto>();
            CreateMap<Reglamento, ReglamentoDto>();
            CreateMap<SectorProducto, SectorProductoDto>();
            CreateMap<Actividad, ActividadDto>();
            CreateMap<Objetivo, ObjetivoDto>();
            CreateMap<Organismo, OrganismoDto>();
            CreateMap<OrganosCodigoAdmin, OrganosCodigoAdminDto>();
            CreateMap<ConfiguracionMicroportal, ConfiguracionMicroportalDto>();
            CreateMap<EnlaceMicroVentana, EnlaceMicroVentanaDto>();
            CreateMap<Suscripcion, SuscripcionDto>();
            CreateMap<Beneficiario, BeneficiarioDto>();
        }
    }
}