using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class MinimisService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MinimisService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<MinimisDto>> GetAllAsync()
        {
            var minimis = await _unitOfWork.Minimis.GetAllAsync();
            return minimis.Select(m => new MinimisDto
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Descripcion = m.Descripcion,
                Estado = m.Estado,
                FechaInicio = m.FechaInicio,
                FechaFin = m.FechaFin
            });
        }
    }
}