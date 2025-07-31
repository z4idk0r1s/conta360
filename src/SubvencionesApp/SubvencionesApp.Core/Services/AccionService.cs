using SubvencionesApp.Core.Dtos;
using SubvencionesApp.Core.Entities;
using SubvencionesApp.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Core.Services
{
    public class AccionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AccionDto>> GetAllAsync()
        {
            var acciones = await _unitOfWork.Acciones.GetAllAsync();
            return acciones.Select(a => new AccionDto { Id = a.Id, Descripcion = a.Descripcion });
        }
    }
}