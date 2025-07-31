using SubvencionesApp.Core.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubvencionesApp.Core.Interfaces
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
    }
}