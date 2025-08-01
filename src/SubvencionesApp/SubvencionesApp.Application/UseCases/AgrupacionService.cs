using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class AgrupacionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AgrupacionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AgrupacionDto>> GetAllAsync()
        {
            var agrupaciones = await _unitOfWork.Agrupaciones.GetAllAsync();
            return agrupaciones.Select(a => new AgrupacionDto { Id = a.Id, Descripcion = a.Descripcion });
        }
    }
}