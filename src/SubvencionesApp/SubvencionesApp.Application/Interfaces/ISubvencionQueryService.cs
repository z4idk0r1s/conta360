using SubvencionesApp.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace SubvencionesApp.Application.Interfaces
{
    public interface ISubvencionQueryService
    {
        Task<IEnumerable<ConvocatoriaDto>> GetConvocatoriasAsync();
        Task<IEnumerable<ConcesionDto>> GetConcesionesAsync();
        Task<IEnumerable<BeneficiarioDto>> GetBeneficiariosAsync();
        Task<ConvocatoriaDto?> GetConvocatoriaByIdAsync(Guid id);
        Task<ConcesionDto?> GetConcesionByIdAsync(Guid id);
        Task<BeneficiarioDto?> GetBeneficiarioByIdAsync(Guid id);
        // 
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
        Task<IEnumerable<TipoBeneficiarioDto>> GetTiposBeneficiarioAsync();
    }
}