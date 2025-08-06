using SubvencionesApp.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.Interfaces
{
    public interface IExternalSubvencionesService
    {
        Task<List<ConvocatoriaDto>> GetConvocatoriasAsync();
        Task<List<ConcesionDto>> GetConcesionesAsync();
        Task<List<BeneficiarioDto>> GetBeneficiariosAsync();
        Task<List<AccionDto>> GetAccionesAsync();
        Task<List<AgrupacionDto>> GetAgrupacionesAsync();
        Task<List<AreaDto>> GetAreasAsync();
        Task<List<EntidadDto>> GetEntidadesAsync();
        Task<List<EstadoDto>> GetEstadosAsync();
        Task<List<FormaPagoDto>> GetFormasPagoAsync();
        Task<List<LineaDto>> GetLineasAsync();
        Task<List<MunicipioDto>> GetMunicipiosAsync();
        Task<List<OrganismoDto>> GetOrganismosAsync();
        Task<List<ProgramaDto>> GetProgramasAsync();
        Task<List<ProvinciaDto>> GetProvinciasAsync();
        Task<List<SectorDto>> GetSectoresAsync();
        Task<List<SituacionEntornoDto>> GetSituacionesEntornoAsync();
        Task<List<SubtipoSubvencionDto>> GetSubtiposSubvencionAsync();
        Task<List<TipoBeneficiarioDto>> GetTiposBeneficiarioAsync();
        Task<List<TipoConvocatoriaDto>> GetTiposConvocatoriaAsync();
        Task<List<TipoOrganismoDto>> GetTiposOrganismoAsync();
        Task<List<TipoSubvencionDto>> GetTiposSubvencionAsync();
        Task<List<TramoDto>> GetTramosAsync();
        Task<List<UnidadAdministrativaDto>> GetUnidadesAdministrativasAsync();
        Task<List<DatosEstadisticosDto>> GetDatosEstadisticosAsync();

        Task<List<AyudaDto>> GetAyudasAsync();
        Task<List<AyudaEstadoDto>> GetAyudasEstadosAsync();
        Task<List<ConcesionDetalleDto>> GetConcesionesDetalleAsync();
        Task<List<ConvocatoriaDetalleDto>> GetConvocatoriasDetalleAsync();
        Task<List<FinalidadDto>> GetFinalidadesAsync();
        Task<List<GrandeBeneficiarioDto>> GetGrandesBeneficiariosAsync();
        Task<List<InstrumentoDto>> GetInstrumentosAsync();
        Task<List<MinimisDto>> GetMinimisAsync();
        Task<List<ObjetivoDto>> GetObjetivosAsync();
        Task<List<OrganosCodigoAdminDto>> GetOrganosCodigoAdminAsync();
        Task<List<PartidoPoliticoDto>> GetPartidosPoliticosAsync();
        Task<List<PlanEstrategicoDto>> GetPlanesEstrategicosAsync();
        Task<List<PlanEstrategicoDetalleDto>> GetPlanesEstrategicosDetalleAsync();
        Task<List<PlazoDto>> GetPlazosAsync();
        Task<List<RegionDto>> GetRegionesAsync();
        Task<List<ReglamentoDto>> GetReglamentosAsync();
        Task<List<SancionDto>> GetSancionesAsync();
        Task<List<SancionDetalleDto>> GetSancionesDetalleAsync();
        Task<List<SectorProductoDto>> GetSectoresProductosAsync();
        Task<List<SuscripcionDto>> GetSuscripcionesAsync();
        Task<List<TerceroDto>> GetTercerosAsync();
    }
}