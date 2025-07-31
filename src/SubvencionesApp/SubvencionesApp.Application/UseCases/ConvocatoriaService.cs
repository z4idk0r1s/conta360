using SubvencionesApp.Domain.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class ConvocatoriaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConvocatoriaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ConvocatoriaDto>> GetAllAsync()
        {
            var convocatorias = await _unitOfWork.Convocatorias.GetAllAsync();
            return convocatorias.Select(c => new ConvocatoriaDto { Id = c.Id, Objeto = c.Objeto, Extracto = c.Extracto, Enlace = c.Enlace, ReferenciaBDNS = c.ReferenciaBDNS, Ejercicio = c.Ejercicio, FechaPublicacion = c.FechaPublicacion });
        }
    }
}