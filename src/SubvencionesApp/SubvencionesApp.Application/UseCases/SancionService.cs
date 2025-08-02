using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class SancionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SancionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SancionDto>> GetAllAsync()
        {
            var sanciones = await _unitOfWork.Sanciones.GetAllAsync();
            return sanciones.Select(s => new SancionDto
            {
                Id = s.Id,
                Nombre = s.Nombre,
                Motivo = s.Motivo,
                Sancion = s.Sancion,
                Estado = s.Estado
            });
        }
    }
}