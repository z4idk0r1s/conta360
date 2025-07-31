using SubvencionesApp.Core.Dtos;
using SubvencionesApp.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Core.Services
{
    public class TramoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TramoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TramoDto>> GetAllAsync()
        {
            var tramos = await _unitOfWork.Tramos.GetAllAsync();
            return tramos.Select(t => new TramoDto { Id = t.Id, Descripcion = t.Descripcion });
        }
    }
}