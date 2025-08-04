using AutoMapper;
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Infrastructure.ExternalServices.Models;

namespace SubvencionesApp.Infrastructure.ExternalServices.Mappings
{
    public partial class MappingProfile : Profile
    {
        // ===========================================
        // 2. API → DTO
        // ===========================================
        private void ConfigureApiToDtoMappings()
        {
            // Mapeos de modelos de la API a DTOs.
            // La propiedad 'Id' de la API se mapea a 'ExternalId' en el DTO.
            CreateMap<ConvocatoriaApiModel, ConvocatoriaDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<ConvocatoriaDetalleApiModel, ConvocatoriaDetalleDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<ConcesionApiModel, ConcesionDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
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
            CreateMap<OrganoApiModel, OrganoDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<OrganosCodigoAdminApiModel, OrganosCodigoAdminDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<ConfiguracionMicroportalApiModel, ConfiguracionMicroportalDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<EnlaceMicroVentanaApiModel, EnlaceMicroVentanaDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<SuscripcionApiModel, SuscripcionDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<BeneficiarioApiModel, BeneficiarioDto>().ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
        }
    }
}