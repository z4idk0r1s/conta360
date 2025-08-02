using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class RegionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RegionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RegionDto>> GetAllAsync()
        {
            var regiones = await _unitOfWork.Regiones.GetAllAsync();
            return regiones.Select(r => new RegionDto
            {
                Id = r.Id,
                Nombre = r.Nombre
            });
        }
    }
}