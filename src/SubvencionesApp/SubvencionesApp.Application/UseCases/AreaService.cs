using SubvencionesApp.Domain.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Domain.Services
{
    public class AreaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AreaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AreaDto>> GetAllAsync()
        {
            var areas = await _unitOfWork.Areas.GetAllAsync();
            return areas.Select(a => new AreaDto { Id = a.Id, Descripcion = a.Descripcion });
        }
    }
}