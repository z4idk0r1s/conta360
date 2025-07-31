using SubvencionesApp.Domain.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class ConcesionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ConcesionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ConcesionDto>> GetAllAsync()
        {
            var concesiones = await _unitOfWork.Concesiones.GetAllAsync();
            return concesiones.Select(c => new ConcesionDto { IdConcesion = c.IdConcesion, ReferenciaBDNS = c.ReferenciaBDNS, ReferenciaPublicacion = c.ReferenciaPublicacion, Importe = c.Importe, Ejercicio = c.Ejercicio, FechaConcesion = c.FechaConcesion });
        }
    }
}