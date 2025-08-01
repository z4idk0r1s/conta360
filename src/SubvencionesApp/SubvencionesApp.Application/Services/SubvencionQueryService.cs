using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.Services
{
    public class SubvencionQueryService : ISubvencionQueryService
    {
        public Task<IEnumerable<ConvocatoriaDto>> GetConvocatoriasAsync()
        {
            // TODO: Implementa la lógica para obtener las convocatorias desde la base de datos o un servicio
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ConcesionDto>> GetConcesionesAsync()
        {
            // TODO: Implementa la lógica para obtener las concesiones
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BeneficiarioDto>> GetBeneficiariosAsync()
        {
            // TODO: Implementa la lógica para obtener los beneficiarios
            throw new NotImplementedException();
        }

        public Task<ConvocatoriaDto?> GetConvocatoriaByIdAsync(long id)
        {
            // TODO: Implementa la lógica para obtener una convocatoria por ID
            throw new NotImplementedException();
        }

        public Task<ConcesionDto?> GetConcesionByIdAsync(long id)
        {
            // TODO: Implementa la lógica para obtener una concesión por ID
            throw new NotImplementedException();
        }

        public Task<BeneficiarioDto?> GetBeneficiarioByIdAsync(long id)
        {
            // TODO: Implementa la lógica para obtener un beneficiario por ID
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ConvocatoriaDto>> SearchConvocatoriasByTextAsync(string searchText)
        {
            // TODO: Implementa la lógica de búsqueda de convocatorias por texto
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ConcesionDto>> GetConcesionesByEjercicioAsync(int ejercicio)
        {
            // TODO: Implementa la lógica para obtener concesiones por ejercicio
            throw new NotImplementedException();
        }

        public Task<DatosEstadisticosDto> GetEstadisticasGeneralesAsync()
        {
            // TODO: Implementa la lógica para obtener estadísticas generales
            throw new NotImplementedException();
        }
    }
}