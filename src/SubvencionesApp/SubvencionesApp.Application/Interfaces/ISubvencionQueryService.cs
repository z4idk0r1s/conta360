using SubvencionesApp.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.Interfaces
{
    public interface ISubvencionQueryService
    {
        Task<IEnumerable<ConvocatoriaDto>> GetConvocatoriasAsync();
        Task<IEnumerable<ConcesionDto>> GetConcesionesAsync();
        Task<IEnumerable<BeneficiarioDto>> GetBeneficiariosAsync();
        Task<ConvocatoriaDto?> GetConvocatoriaByIdAsync(long id);
        Task<ConcesionDto?> GetConcesionByIdAsync(long id);
        Task<BeneficiarioDto?> GetBeneficiarioByIdAsync(long id);
        Task<IEnumerable<ConvocatoriaDto>> SearchConvocatoriasByTextAsync(string searchText);
        Task<IEnumerable<ConcesionDto>> GetConcesionesByEjercicioAsync(int ejercicio);
        Task<DatosEstadisticosDto> GetEstadisticasGeneralesAsync();
    }
}