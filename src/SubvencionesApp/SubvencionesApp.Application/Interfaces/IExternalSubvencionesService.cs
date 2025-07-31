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
    }
}