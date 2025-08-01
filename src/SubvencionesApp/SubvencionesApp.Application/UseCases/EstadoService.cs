using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class EstadoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EstadoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EstadoDto>> GetAllAsync()
        {
            var estados = await _unitOfWork.Estados.GetAllAsync();
            return estados.Select(e => new EstadoDto { Id = e.Id, Descripcion = e.Descripcion });
        }
    }
}