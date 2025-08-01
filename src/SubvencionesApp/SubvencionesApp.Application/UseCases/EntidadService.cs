using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class EntidadService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EntidadService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<EntidadDto>> GetAllAsync()
        {
            var entidades = await _unitOfWork.Entidades.GetAllAsync();
            return entidades.Select(e => new EntidadDto { Id = e.Id, Descripcion = e.Descripcion });
        }
    }
}