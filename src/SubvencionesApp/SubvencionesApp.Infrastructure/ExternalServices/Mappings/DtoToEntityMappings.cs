using AutoMapper;
using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Entities;

namespace SubvencionesApp.Infrastructure.ExternalServices.Mappings
{
    public partial class MappingProfile : Profile
    {
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
            CreateMap<OrganoDto, Organo>();
            CreateMap<OrganosCodigoAdminDto, OrganosCodigoAdmin>();
            CreateMap<ConfiguracionMicroportalDto, ConfiguracionMicroportal>();
            CreateMap<EnlaceMicroVentanaDto, EnlaceMicroVentana>();
            CreateMap<SuscripcionDto, Suscripcion>();
            CreateMap<BeneficiarioDto, Beneficiario>();
        }
    }
}