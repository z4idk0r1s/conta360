using AutoMapper;
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Infrastructure.ExternalServices.Models;
using System;

namespace SubvencionesApp.Infrastructure.ExternalServices
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ================================
            // ✅ 1. Conversiones globales seguras y completas
            // ================================

            // Guid -> string
            CreateMap<Guid, string>().ConvertUsing(src => src.ToString());

            // Guid? -> string
            CreateMap<Guid?, string>().ConvertUsing(src => src.HasValue ? src.Value.ToString() : string.Empty);

            // string -> Guid
            CreateMap<string, Guid>().ConvertUsing(src => Guid.TryParse(src, out var guid) ? guid : Guid.Empty);

            // string -> Guid?
            CreateMap<string, Guid?>().ConvertUsing(src => Guid.TryParse(src, out var guid) ? guid : (Guid?)null);

            // DateTime -> string (formato ISO simple)
            CreateMap<DateTime, string>().ConvertUsing(src => src.ToString("yyyy-MM-dd"));

            // DateTime? -> string
            CreateMap<DateTime?, string>().ConvertUsing(src => src.HasValue ? src.Value.ToString("yyyy-MM-dd") : string.Empty);

            // string -> DateTime
            CreateMap<string, DateTime>().ConvertUsing(src => DateTime.TryParse(src, out var date) ? date : default);

            // string -> DateTime?
            CreateMap<string, DateTime?>().ConvertUsing(src => DateTime.TryParse(src, out var date) ? date : (DateTime?)null);

            // decimal -> decimal?
            CreateMap<decimal, decimal?>().ConvertUsing(src => (decimal?)src);

            // decimal? -> decimal (valor por defecto 0m)
            CreateMap<decimal?, decimal>().ConvertUsing(src => src ?? 0m);

            // int -> int?
            CreateMap<int, int?>().ConvertUsing(src => (int?)src);

            // int? -> int (valor por defecto 0)
            CreateMap<int?, int>().ConvertUsing(src => src ?? 0);

            // ================================
            // ✅ 2. Mappeos específicos con ExternalId para evitar problemas de tipos
            // ================================

            CreateMap<ConvocatoriaApiModel, ConvocatoriaDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ConvocatoriaDetalleApiModel, ConvocatoriaDetalleDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ConcesionApiModel, ConcesionDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ConcesionDetalleApiModel, ConcesionDetalleDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<AyudaApiModel, AyudaDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<AyudaEstadoApiModel, AyudaEstadoDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<PlazoApiModel, PlazoDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<MinimisApiModel, MinimisDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<GrandeBeneficiarioApiModel, GrandeBeneficiarioDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<PartidoPoliticoApiModel, PartidoPoliticoDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<PlanEstrategicoApiModel, PlanEstrategicoDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<PlanEstrategicoDetalleApiModel, PlanEstrategicoDetalleDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<SancionApiModel, SancionDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<SancionDetalleApiModel, SancionDetalleDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<TerceroApiModel, TerceroDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<RegionApiModel, RegionDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<FinalidadApiModel, FinalidadDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<TipoBeneficiarioApiModel, TipoBeneficiarioDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<InstrumentoApiModel, InstrumentoDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ReglamentoApiModel, ReglamentoDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<SectorProductoApiModel, SectorProductoDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ActividadApiModel, ActividadDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ObjetivoApiModel, ObjetivoDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<OrganoApiModel, OrganoDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<OrganosCodigoAdminApiModel, OrganosCodigoAdminDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ConfiguracionMicroportalApiModel, ConfiguracionMicroportalDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<EnlaceMicroVentanaApiModel, EnlaceMicroVentanaDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<SuscripcionApiModel, SuscripcionDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<BeneficiarioApiModel, BeneficiarioDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            // ================================
            // ✅ 3. Asegurar mapeo de propiedades anidadas y listas
            // ================================
            // Nota: Todos los ApiModel usados como propiedades están mapeados arriba.
            // Si agregas nuevos objetos anidados, asegúrate de crear su CreateMap.

            // ================================
            // ⚠️ 4. Recuerda manejar referencias circulares y Lazy Loading en la consulta y uso del mapper:
            //    - Usa .ForMember(..., opt => opt.Ignore()) para romper ciclos.
            //    - Usa Include() para evitar N+1 queries con EF.
            //    - Usa ProjectTo<> para consultas optimizadas cuando sea posible.
            // ================================
        }
    }
}
