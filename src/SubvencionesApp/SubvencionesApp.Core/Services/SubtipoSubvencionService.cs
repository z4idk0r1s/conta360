using SubvencionesApp.Core.Dtos;
using SubvencionesApp.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Core.Services
{
    public class SubtipoSubvencionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubtipoSubvencionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SubtipoSubvencionDto>> GetAllAsync()
        {
            var subtipos = await _unitOfWork.SubtiposSubvencion.GetAllAsync();
            return subtipos.Select(s => new SubtipoSubvencionDto { Id = s.Id, Descripcion = s.Descripcion });
        }
    }
}