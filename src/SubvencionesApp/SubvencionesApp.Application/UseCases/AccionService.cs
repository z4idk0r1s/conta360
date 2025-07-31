using SubvencionesApp.Domain.Dtos;
using SubvencionesApp.Domain.Entities;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
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