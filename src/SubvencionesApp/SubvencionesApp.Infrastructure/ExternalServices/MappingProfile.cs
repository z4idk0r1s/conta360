using AutoMapper;
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Infrastructure.ExternalServices.Models;

namespace SubvencionesApp.Infrastructure.ExternalServices
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ConvocatoriaApiModel, ConvocatoriaDto>();
            CreateMap<ConvocatoriaDetalleApiModel, ConvocatoriaDetalleDto>();
            CreateMap<ConcesionApiModel, ConcesionDto>();
            CreateMap<ConcesionDetalleApiModel, ConcesionDetalleDto>();
            CreateMap<AyudaApiModel, AyudaDto>();
            CreateMap<AyudaEstadoApiModel, AyudaEstadoDto>();
            CreateMap<PlazoApiModel, PlazoDto>();
            CreateMap<MinimisApiModel, MinimisDto>();
            CreateMap<GrandeBeneficiarioApiModel, GrandeBeneficiarioDto>();
            CreateMap<PartidoPoliticoApiModel, PartidoPoliticoDto>();
            CreateMap<PlanEstrategicoApiModel, PlanEstrategicoDto>();
            CreateMap<PlanEstrategicoDetalleApiModel, PlanEstrategicoDetalleDto>();
            CreateMap<SancionApiModel, SancionDto>();
            CreateMap<SancionDetalleApiModel, SancionDetalleDto>();
            CreateMap<TerceroApiModel, TerceroDto>();
            CreateMap<RegionApiModel, RegionDto>();
            CreateMap<FinalidadApiModel, FinalidadDto>();
            CreateMap<TipoBeneficiarioApiModel, TipoBeneficiarioDto>();
            CreateMap<InstrumentoApiModel, InstrumentoDto>();
            CreateMap<ReglamentoApiModel, ReglamentoDto>();
            CreateMap<SectorProductoApiModel, SectorProductoDto>();
            CreateMap<ActividadApiModel, ActividadDto>();
            CreateMap<ObjetivoApiModel, ObjetivoDto>();
            CreateMap<OrganoApiModel, OrganoDto>();
            CreateMap<OrganosCodigoAdminApiModel, OrganosCodigoAdminDto>();
            CreateMap<ConfiguracionMicroportalApiModel, ConfiguracionMicroportalDto>();
            CreateMap<EnlaceMicroVentanaApiModel, EnlaceMicroVentanaDto>();
            CreateMap<SuscripcionApiModel, SuscripcionDto>();
            CreateMap<BeneficiarioApiModel, BeneficiarioDto>();
        }
    }
}
