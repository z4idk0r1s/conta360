using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
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