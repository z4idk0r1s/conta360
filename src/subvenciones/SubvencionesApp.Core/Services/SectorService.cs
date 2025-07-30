using SubvencionesApp.Core.Dtos;
using SubvencionesApp.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Core.Services
{
    public class SectorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SectorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SectorDto>> GetAllAsync()
        {
            var sectores = await _unitOfWork.Sectores.GetAllAsync();
            return sectores.Select(s => new SectorDto { Id = s.Id, Descripcion = s.Descripcion });
        }
    }
}