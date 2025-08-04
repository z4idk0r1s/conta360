using AutoMapper;
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Entities;

namespace SubvencionesApp.Infrastructure.ExternalServices.Mappings
{
    public partial class MappingProfile : Profile
    {
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
            CreateMap<Organo, OrganoDto>();
            CreateMap<OrganosCodigoAdmin, OrganosCodigoAdminDto>();
            CreateMap<ConfiguracionMicroportal, ConfiguracionMicroportalDto>();
            CreateMap<EnlaceMicroVentana, EnlaceMicroVentanaDto>();
            CreateMap<Suscripcion, SuscripcionDto>();
            CreateMap<Beneficiario, BeneficiarioDto>();
        }
    }
}