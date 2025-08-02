using SubvencionesApp.Application.Dtos;
using SubvencionesApp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubvencionesApp.Application.UseCases
{
    public class SancionDetalleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SancionDetalleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SancionDetalleDto>> GetAllAsync()
        {
            var sancionesDetalle = await _unitOfWork.SancionesDetalle.GetAllAsync();
            return sancionesDetalle.Select(sd => new SancionDetalleDto
            {
                Id = sd.Id,
                Nombre = sd.Nombre,
                Motivo = sd.Motivo,
                Sancion = sd.Sancion,
                Estado = sd.Estado,
                Detalles = sd.Detalles,
                FechaResolucion = sd.FechaResolucion,
                OrganismoId = sd.OrganismoId
            });
        }
    }
}