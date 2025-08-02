using SubvencionesApp.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubvencionesApp.Infrastructure.Interfaces
{
    public interface ISubvencionesService
    {
        Task<IEnumerable<AccionDto>> GetAccionesAsync();
        Task<IEnumerable<AgrupacionDto>> GetAgrupacionesAsync();
        Task<IEnumerable<AreaDto>> GetAreasAsync();
        Task<IEnumerable<BeneficiarioDto>> GetBeneficiariosAsync();
        Task<IEnumerable<ConcesionDto>> GetConcesionesAsync();
        Task<IEnumerable<ConvocatoriaDto>> GetConvocatoriasAsync();
        Task<IEnumerable<DatosEstadisticosDto>> GetDatosEstadisticosAsync();
        Task<IEnumerable<EntidadDto>> GetEntidadesAsync();
        Task<IEnumerable<EstadoDto>> GetEstadosAsync();
        Task<IEnumerable<FormaPagoDto>> GetFormasPagoAsync();
        Task<IEnumerable<LineaDto>> GetLineasAsync();
        Task<IEnumerable<MunicipioDto>> GetMunicipiosAsync();
        Task<IEnumerable<OrganismoDto>> GetOrganismosAsync();
        Task<IEnumerable<ProgramaDto>> GetProgramasAsync();
        Task<IEnumerable<ProvinciaDto>> GetProvinciasAsync();
        Task<IEnumerable<SectorDto>> GetSectoresAsync();
        Task<IEnumerable<SituacionEntornoDto>> GetSituacionesEntornoAsync();
        Task<IEnumerable<SubtipoSubvencionDto>> GetSubtiposSubvencionAsync();
        Task<IEnumerable<TipoBeneficiarioDto>> GetTiposBeneficiarioAsync();
        Task<IEnumerable<TipoConvocatoriaDto>> GetTiposConvocatoriaAsync();
        Task<IEnumerable<TipoOrganismoDto>> GetTiposOrganismoAsync();
        Task<IEnumerable<TipoSubvencionDto>> GetTiposSubvencionAsync();
        Task<IEnumerable<TramoDto>> GetTramosAsync();
        Task<IEnumerable<UnidadAdministrativaDto>> GetUnidadesAdministrativasAsync();

        Task<IEnumerable<AyudaDto>> GetAyudasAsync();
        Task<IEnumerable<AyudaEstadoDto>> GetAyudasEstadosAsync();
        Task<IEnumerable<ConcesionDetalleDto>> GetConcesionesDetalleAsync();
        Task<IEnumerable<ConvocatoriaDetalleDto>> GetConvocatoriasDetalleAsync();
        Task<IEnumerable<FinalidadDto>> GetFinalidadesAsync();
        Task<IEnumerable<GrandeBeneficiarioDto>> GetGrandesBeneficiariosAsync();
        Task<IEnumerable<InstrumentoDto>> GetInstrumentosAsync();
        Task<IEnumerable<MinimisDto>> GetMinimisAsync();
        Task<IEnumerable<ObjetivoDto>> GetObjetivosAsync();
        Task<IEnumerable<OrganosCodigoAdminDto>> GetOrganosCodigoAdminAsync();
        Task<IEnumerable<PartidoPoliticoDto>> GetPartidosPoliticosAsync();
        Task<IEnumerable<PlanEstrategicoDto>> GetPlanesEstrategicosAsync();
        Task<IEnumerable<PlanEstrategicoDetalleDto>> GetPlanesEstrategicosDetalleAsync();
        Task<IEnumerable<PlazoDto>> GetPlazosAsync();
        Task<IEnumerable<RegionDto>> GetRegionesAsync();
        Task<IEnumerable<ReglamentoDto>> GetReglamentosAsync();
        Task<IEnumerable<SancionDto>> GetSancionesAsync();
        Task<IEnumerable<SancionDetalleDto>> GetSancionesDetalleAsync();
        Task<IEnumerable<SectorProductoDto>> GetSectoresProductosAsync();
        Task<IEnumerable<SuscripcionDto>> GetSuscripcionesAsync();
        Task<IEnumerable<TerceroDto>> GetTercerosAsync();
    }
}