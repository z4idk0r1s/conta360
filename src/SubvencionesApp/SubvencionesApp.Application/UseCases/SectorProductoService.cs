using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class SectorProductoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SectorProductoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SectorProductoDto>> GetAllAsync()
        {
            var sectoresProductos = await _unitOfWork.SectoresProductos.GetAllAsync();
            return sectoresProductos.Select(sp => new SectorProductoDto
            {
                Id = sp.Id,
                Nombre = sp.Nombre,
                Descripcion = sp.Descripcion
            });
        }
    }
}