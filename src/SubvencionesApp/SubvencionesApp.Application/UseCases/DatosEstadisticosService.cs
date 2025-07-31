using SubvencionesApp.Domain.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Domain.Services
{
    public class DatosEstadisticosService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DatosEstadisticosService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DatosEstadisticosDto>> GetAllAsync()
        {
            var datos = await _unitOfWork.DatosEstadisticos.GetAllAsync();
            return datos.Select(d => new DatosEstadisticosDto { Id = d.Id, Descripcion = d.Descripcion, TotalConcesiones = d.TotalConcesiones, ImporteTotal = d.ImporteTotal });
        }
    }
}